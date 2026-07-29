using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Contracts.General;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;

namespace Itera.BookingService.Application.Estimate;

public sealed class AmountEstimateParityService(
    IEstimateAccessoryQueryService estimateAccessoryQueryService,
    IEstimateInsuranceQueryService estimateInsuranceQueryService,
    IEstimateAmountTokenQueryService estimateAmountTokenQueryService,
    ILogger<AmountEstimateParityService> logger) : IAmountEstimateParityService
{
    public async Task<ApiResponse<AmountEstimateDto>> GetAmountEstimateAsync(
        GetAmountEstimateRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken)
    {
        // Legacy fallback from WsValidate.ValidateEstimateToken: 300 seconds when config is missing.
        var estimateTokenValidation = await estimateAmountTokenQueryService.ValidateEstimateTokenAsync(
            request.EstimateToken,
            tokenValidPeriodSeconds: 300,
            cancellationToken);

        if (estimateTokenValidation.ValidationCode < 0)
        {
            return estimateTokenValidation.ValidationCode switch
            {
                -1 => LegacyError<AmountEstimateDto>(-312, "Estimate token already in use"),
                -2 => LegacyError<AmountEstimateDto>(-101, "Estimate token has expired"),
                _ => LegacyError<AmountEstimateDto>(-309, "An error occurred while retrieving the estimate")
            };
        }

        var snapshot = estimateTokenValidation.Snapshot;
        if (snapshot is null)
            return LegacyError<AmountEstimateDto>(-309, "An error occurred while retrieving the estimate");

        var dyn = TryDeserializeObjectDynParam(snapshot.ObjectDynParam);
        if (dyn?.StateSegment is null || dyn.KmType is null)
            return LegacyError<AmountEstimateDto>(-309, "An error occurred while retrieving the estimate");

        if (!dyn.StateSegment.TryGetValue(request.SegmentCode, out var segmentState))
            return LegacyError<AmountEstimateDto>(-236, "Unable to retrieve segment information");

        var acceptsNonSellable = await estimateAmountTokenQueryService.AcceptsNonSellableSegmentAsync(
            authContext.WsUserId,
            cancellationToken);

        if (string.Equals(segmentState, "N", StringComparison.OrdinalIgnoreCase) && !acceptsNonSellable)
            return LegacyError<AmountEstimateDto>(-236, "Unable to retrieve segment information");

        if (!dyn.KmType.TryGetValue(request.KmType, out var kmId) || kmId == 0)
            return LegacyError<AmountEstimateDto>(-310, "Unable to retrieve km information");

        var ivaId = await estimateAmountTokenQueryService.GetCurrentIvaIdAsync(cancellationToken);
        if (!ivaId.HasValue || ivaId.Value <= 0)
            return LegacyError<AmountEstimateDto>(-313, "Unable to retrieve VAT information");

        var insuranceList = request.InsuranceList;
        var insuranceExtraList = request.InsuranceExtraList;
        if ((insuranceExtraList is not null && insuranceExtraList.Count > 0)
            || (insuranceList is not null && insuranceList.Count > 0))
        {
            if (!snapshot.Giorni.HasValue || !snapshot.ListinoId.HasValue)
                return LegacyError<AmountEstimateDto>(-323, "Unable to retrieve insurance list");

            var insuranceOptions = await estimateAmountTokenQueryService.GetInsuranceOptionsAsync(
                request.SegmentCode,
                snapshot.Giorni.Value,
                snapshot.ListinoId.Value,
                snapshot.DataFromPreventivo,
                snapshot.DataToPreventivo,
                cancellationToken);

            if (insuranceExtraList is not null && insuranceExtraList.Count > 0)
            {
                var selectedIds = insuranceExtraList.ToHashSet();
                var availableIds = insuranceOptions.Select(i => i.ListinoFranchigiaId).ToHashSet();
                if (!selectedIds.IsSubsetOf(availableIds))
                    return LegacyError<AmountEstimateDto>(-323, "Unable to retrieve insurance list");
            }

            if (insuranceList is not null && insuranceList.Count > 0)
            {
                var selectedType = insuranceList.First().Type;
                var availableTypes = insuranceOptions
                    .Select(i => i.TipologiaFranchigiaId)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                if (string.IsNullOrWhiteSpace(selectedType) || !availableTypes.Contains(selectedType))
                    return LegacyError<AmountEstimateDto>(-323, "Unable to retrieve insurance list");
            }
        }

        var accessoryList = request.AccessoryList?.ToList() ?? [];
        var includedAccessories = dyn.Accessory?
            .Where(x => x.Key == request.SegmentCode)
            .SelectMany(x => x.Value)
            .ToList();

        if (includedAccessories is not null && includedAccessories.Count > 0)
        {
            accessoryList.RemoveAll(x => includedAccessories.Contains(x.AccessoryID));
            foreach (var accessoryId in includedAccessories)
                accessoryList.Add(new AmountEstimateAccessoryRequest { AccessoryID = accessoryId, Quantity = 1 });
        }

        if (accessoryList.Count > 0)
        {
            var requestedAccessoryIds = accessoryList.Select(x => x.AccessoryID).ToList();
            var accessoryCodes = await estimateAmountTokenQueryService.GetAccessoryCodesByIdsAsync(
                requestedAccessoryIds,
                cancellationToken);

            if (requestedAccessoryIds.Count != accessoryCodes.Count)
                return LegacyError<AmountEstimateDto>(-325, "Unable to retrieve accessories");

            foreach (var accessory in accessoryList)
            {
                if (!accessoryCodes.TryGetValue(accessory.AccessoryID, out var code))
                    return LegacyError<AmountEstimateDto>(-325, "Unable to retrieve accessories");

                var maxQuantity = GetAccessoryMaxQuantity(code);
                if (accessory.Quantity > maxQuantity)
                    return LegacyError<AmountEstimateDto>(-326, $"Errore quantità max accessorio({code}) superata");
            }
        }

        var parsedAmount = TryExtractAmountEstimate(snapshot.ObjectEstimate, request.SegmentCode, request.KmType);
        if (parsedAmount is null)
            return LegacyError<AmountEstimateDto>(-321, "Unable to retrieve rates");

        if (!string.IsNullOrWhiteSpace(request.DiscountCode)
            && !ValidateRequestedDiscountCode(request.DiscountCode!, snapshot.VoucherCliente, parsedAmount.Discount, out var discountValidationMessage))
        {
            return LegacyError<AmountEstimateDto>(-324, discountValidationMessage ?? "Discount code not valid");
        }

        var hasExplicitAccessoryRequest = request.AccessoryList is { Count: > 0 };
        var hasExplicitInsuranceRequest =
            (request.InsuranceExtraList is { Count: > 0 })
            || (request.InsuranceList is { Count: > 0 });

        AmountEstimateDto amountResult;
        if (!hasExplicitAccessoryRequest && !hasExplicitInsuranceRequest)
        {
            amountResult = parsedAmount.ToDto();
        }
        else
        {
            var ivaPercentage = await estimateAmountTokenQueryService.GetCurrentIvaPercentageAsync(cancellationToken);
            if (!ivaPercentage.HasValue)
                return LegacyError<AmountEstimateDto>(-313, "Unable to retrieve VAT information");

            decimal insuranceVatMultiplier = 1m + (ivaPercentage.Value / 100m);

            decimal accessoryOldVat = 0m;
            decimal accessoryOldNet = 0m;
            decimal accessoryNewVat = 0m;
            decimal accessoryNewNet = 0m;

            if (hasExplicitAccessoryRequest)
            {
                if (string.IsNullOrWhiteSpace(snapshot.CodiceCategoria)
                    || !snapshot.Giorni.HasValue
                    || !snapshot.ListinoId.HasValue)
                {
                    return LegacyError<AmountEstimateDto>(-321, "Unable to retrieve rates");
                }

                var accessoryPricing = await estimateAccessoryQueryService.GetAccessoryBookingAsync(
                    authContext.BrandId,
                    snapshot.FilialeId,
                    snapshot.FilialeDestinazioneId,
                    snapshot.ListinoId.Value,
                    snapshot.Giorni.Value,
                    snapshot.DataFromPreventivo,
                    snapshot.DataToPreventivo,
                    snapshot.CodiceCategoria,
                    request.SegmentCode,
                    cancellationToken);

                var accessoryPricingMap = accessoryPricing
                    .GroupBy(x => x.AccessoryId)
                    .ToDictionary(g => g.Key, g => g.First());

                foreach (var old in parsedAmount.AccessoriesBought)
                {
                    if (accessoryPricingMap.TryGetValue(old.AccessoryId, out var p))
                    {
                        accessoryOldVat += p.AmountVat * old.Quantity;
                        accessoryOldNet += p.Amount * old.Quantity;
                    }
                    else if (old.AmountVat.HasValue)
                    {
                        accessoryOldVat += old.AmountVat.Value;
                        accessoryOldNet += old.AmountVat.Value / insuranceVatMultiplier;
                    }
                }

                foreach (var req in accessoryList)
                {
                    if (!accessoryPricingMap.TryGetValue(req.AccessoryID, out var p))
                        return LegacyError<AmountEstimateDto>(-325, "Unable to retrieve accessories");

                    accessoryNewVat += p.AmountVat * req.Quantity;
                    accessoryNewNet += p.Amount * req.Quantity;
                }
            }

            decimal insuranceOldVat = 0m;
            decimal insuranceOldNet = 0m;
            decimal insuranceNewVat = 0m;
            decimal insuranceNewNet = 0m;

            if (hasExplicitInsuranceRequest)
            {
                if (!snapshot.Giorni.HasValue || !snapshot.ListinoId.HasValue)
                    return LegacyError<AmountEstimateDto>(-321, "Unable to retrieve rates");

                var insurancePricing = await estimateInsuranceQueryService.GetInsuranceExtraAsync(
                    request.SegmentCode,
                    snapshot.DataFromPreventivo,
                    snapshot.DataToPreventivo,
                    snapshot.Giorni.Value,
                    snapshot.ListinoId.Value,
                    cancellationToken);

                var insuranceById = insurancePricing
                    .Where(x => x.InsuranceExtraID > 0)
                    .ToDictionary(x => x.InsuranceExtraID, x => x);

                foreach (var old in parsedAmount.InsurancesBought)
                {
                    if (old.InsuranceExtraId.HasValue && insuranceById.TryGetValue(old.InsuranceExtraId.Value, out var p))
                    {
                        var net = ParseLegacyDecimal(p.InsuranceExtraWithoutIVA);
                        insuranceOldNet += net;
                        insuranceOldVat += net * insuranceVatMultiplier;
                    }
                    else if (old.AmountVat.HasValue)
                    {
                        insuranceOldVat += old.AmountVat.Value;
                        insuranceOldNet += old.AmountVat.Value / insuranceVatMultiplier;
                    }
                }

                IEnumerable<InsuranceExtraDto> selectedInsurance = Enumerable.Empty<InsuranceExtraDto>();
                if (request.InsuranceList is { Count: > 0 })
                {
                    var type = request.InsuranceList[0].Type;
                    selectedInsurance = insurancePricing.Where(x => string.Equals(x.InsuranceExtra, type, StringComparison.OrdinalIgnoreCase));
                }
                else if (request.InsuranceExtraList is { Count: > 0 })
                {
                    var selectedIds = request.InsuranceExtraList.ToHashSet();
                    selectedInsurance = insurancePricing.Where(x => selectedIds.Contains(x.InsuranceExtraID));
                }

                foreach (var ins in selectedInsurance)
                {
                    var net = ParseLegacyDecimal(ins.InsuranceExtraWithoutIVA);
                    insuranceNewNet += net;
                    insuranceNewVat += net * insuranceVatMultiplier;
                }
            }

            var currentAmountVat = ParseLegacyDecimal(parsedAmount.Amounts.Amount);
            var currentAmountNet = ParseLegacyDecimal(parsedAmount.Amounts.AmountWithoutIVA);
            var currentWithoutDiscountVat = ParseLegacyDecimal(parsedAmount.AmountsWithoutDiscount.Amount);
            var currentWithoutDiscountNet = ParseLegacyDecimal(parsedAmount.AmountsWithoutDiscount.AmountWithoutIVA);

            var currentExtrasVat = accessoryOldVat + insuranceOldVat;
            var currentExtrasNet = accessoryOldNet + insuranceOldNet;
            var newExtrasVat = accessoryNewVat + insuranceNewVat;
            var newExtrasNet = accessoryNewNet + insuranceNewNet;

            var recalculatedAmountVat = Math.Max(0m, currentAmountVat - currentExtrasVat + newExtrasVat);
            var recalculatedAmountNet = Math.Max(0m, currentAmountNet - currentExtrasNet + newExtrasNet);
            var recalculatedWithoutDiscountVat = Math.Max(0m, currentWithoutDiscountVat - currentExtrasVat + newExtrasVat);
            var recalculatedWithoutDiscountNet = Math.Max(0m, currentWithoutDiscountNet - currentExtrasNet + newExtrasNet);

            amountResult = new AmountEstimateDto
            {
                Amounts = new AmountEstimateValueDto
                {
                    Amount = FormatLegacyDecimal(recalculatedAmountVat),
                    AmountWithoutIVA = FormatLegacyDecimal(recalculatedAmountNet)
                },
                AmountsWithoutDiscount = new AmountEstimateValueDto
                {
                    Amount = FormatLegacyDecimal(recalculatedWithoutDiscountVat),
                    AmountWithoutIVA = FormatLegacyDecimal(recalculatedWithoutDiscountNet)
                },
                Discount = parsedAmount.Discount
            };
        }

        logger.LogInformation(
            "GetAmountEstimate completed for EstimateToken={EstimateToken}, SegmentCode={SegmentCode}, KmType={KmType}",
            request.EstimateToken,
            request.SegmentCode,
            request.KmType);

        return ApiResponse<AmountEstimateDto>.Ok(amountResult);
    }

    private static ApiResponse<T> LegacyError<T>(int errorCode, string message)
    {
        return new ApiResponse<T>
        {
            Esito = false,
            CodiceErrore = errorCode.ToString(),
            Messaggio = message,
            Data = default
        };
    }

    private static short GetAccessoryMaxQuantity(string accessoryCode)
    {
        return accessoryCode switch
        {
            "OTH" => 3,
            "STS" => 4,
            "OTK" => 1,
            _ => 1
        };
    }

    private static ObjectDynParamDto? TryDeserializeObjectDynParam(string? objectDynParam)
    {
        if (string.IsNullOrWhiteSpace(objectDynParam))
            return null;

        return JsonSerializer.Deserialize<ObjectDynParamDto>(
            objectDynParam,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private static ParsedAmountEstimate? TryExtractAmountEstimate(string? objectEstimate, string segmentCode, string kmType)
    {
        if (string.IsNullOrWhiteSpace(objectEstimate))
            return null;

        using var document = JsonDocument.Parse(objectEstimate);
        var root = document.RootElement;

        if ((!TryGetPropertyIgnoreCase(root, "Segments", out var segments)
            && !TryGetPropertyIgnoreCase(root, "Segmenti", out segments))
            || segments.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        JsonElement? segmentMatch = null;
        foreach (var segment in segments.EnumerateArray())
        {
            if (!TryReadString(segment, "CodeSegment", out var currentSegmentCode)
                && !TryReadString(segment, "SegmentCode", out currentSegmentCode)
                && !TryReadString(segment, "CodiceSegmento", out currentSegmentCode))
            {
                continue;
            }

            if (string.Equals(currentSegmentCode, segmentCode, StringComparison.OrdinalIgnoreCase))
            {
                segmentMatch = segment;
                break;
            }
        }

        if (!segmentMatch.HasValue)
            return null;

        if ((!TryGetPropertyIgnoreCase(segmentMatch.Value, "AmountSegmentEstimate", out var amountsByKm)
            && !TryGetPropertyIgnoreCase(segmentMatch.Value, "ImportiPreventivoSegmento", out amountsByKm))
            || amountsByKm.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        JsonElement? kmMatch = null;
        foreach (var kmAmount in amountsByKm.EnumerateArray())
        {
            if (!TryReadString(kmAmount, "KmType", out var currentKmType)
                && !TryReadString(kmAmount, "KmTypeDescr", out currentKmType))
            {
                continue;
            }

            if (string.Equals(currentKmType, kmType, StringComparison.OrdinalIgnoreCase))
            {
                kmMatch = kmAmount;
                break;
            }
        }

        if (!kmMatch.HasValue)
            return null;

        if ((!TryReadDecimalString(kmMatch.Value, "Amount", out var amount)
             && !TryReadDecimalString(kmMatch.Value, "Importo", out amount))
            || (!TryReadDecimalString(kmMatch.Value, "AmountWithoutIVA", out var amountWithoutIva)
                && !TryReadDecimalString(kmMatch.Value, "ImportoNoIva", out amountWithoutIva)))
        {
            return null;
        }

        AmountEstimateValueDto amountWithoutDiscount;
        if ((TryGetPropertyIgnoreCase(kmMatch.Value, "AmountsWithoutDiscount", out var withoutDiscount)
             || TryGetPropertyIgnoreCase(kmMatch.Value, "ImportiSenzaSconto", out withoutDiscount))
            && withoutDiscount.ValueKind == JsonValueKind.Object
            && (TryReadDecimalString(withoutDiscount, "Amount", out var amountNoDiscount)
                || TryReadDecimalString(withoutDiscount, "Importo", out amountNoDiscount))
            && (TryReadDecimalString(withoutDiscount, "AmountWithoutIVA", out var amountNoDiscountWithoutIva)
                || TryReadDecimalString(withoutDiscount, "ImportoNoIva", out amountNoDiscountWithoutIva)))
        {
            amountWithoutDiscount = new AmountEstimateValueDto
            {
                Amount = amountNoDiscount,
                AmountWithoutIVA = amountNoDiscountWithoutIva
            };
        }
        else
        {
            amountWithoutDiscount = new AmountEstimateValueDto
            {
                Amount = amount,
                AmountWithoutIVA = amountWithoutIva
            };
        }

        List<AmountEstimateDiscountDto>? discounts = null;
        var discountSource = default(JsonElement);
        var hasDiscountSource =
            (TryGetPropertyIgnoreCase(kmMatch.Value, "DiscountList", out discountSource)
             || TryGetPropertyIgnoreCase(kmMatch.Value, "CodiceSconto", out discountSource)
             || TryGetPropertyIgnoreCase(segmentMatch.Value, "DiscountList", out discountSource)
             || TryGetPropertyIgnoreCase(segmentMatch.Value, "CodiceSconto", out discountSource))
            && discountSource.ValueKind == JsonValueKind.Array;

        if (hasDiscountSource)
        {
            discounts = [];
            foreach (var item in discountSource.EnumerateArray())
            {
                discounts.Add(new AmountEstimateDiscountDto
                {
                    HDN_SCN = TryReadDecimal(item, "HDN_SCN")
                              ?? TryReadDecimal(item, "ValoreSconto"),
                    HDN_SCN_KEY = TryReadString(item, "HDN_SCN_KEY", out var key)
                                  ? key
                                  : TryReadString(item, "KeySconto", out key) ? key : null,
                    DiscountTypeID = (short?)TryReadInt32(item, "DiscountTypeID") ?? 0,
                    DiscountTypeDescription = TryReadString(item, "DiscountTypeDescription", out var typeDesc) ? typeDesc : null,
                    RegolaDiVenditaID = TryReadInt32(item, "RegolaDiVenditaID"),
                    DiscountIdError = TryReadInt32(item, "DiscountIdError")
                                      ?? TryReadInt32(item, "IdErrorValidity"),
                    DiscountMessageError = TryReadString(item, "DiscountMessageError", out var errMsg)
                                           ? errMsg
                                           : TryReadString(item, "MessageErrorValidity", out errMsg) ? errMsg : null
                });
            }

            if (discounts.Count == 0)
                discounts = null;
        }

        var accessoriesBought = ExtractAccessoryBought(kmMatch.Value);
        var insurancesBought = ExtractInsuranceBought(kmMatch.Value);

        return new ParsedAmountEstimate(
            new AmountEstimateValueDto
            {
                Amount = amount,
                AmountWithoutIVA = amountWithoutIva
            },
            amountWithoutDiscount,
            discounts,
            accessoriesBought,
            insurancesBought);
    }

    private static List<AccessoryBoughtLine> ExtractAccessoryBought(JsonElement kmNode)
    {
        if ((!TryGetPropertyIgnoreCase(kmNode, "AccessoriesBought", out var bought)
             && !TryGetPropertyIgnoreCase(kmNode, "AccessoriSelezionatiExtra", out bought))
            || bought.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var result = new List<AccessoryBoughtLine>();
        foreach (var item in bought.EnumerateArray())
        {
            var id = TryReadInt32(item, "AccessoryID")
                     ?? TryReadInt32(item, "AccessorioID");
            if (!id.HasValue)
                continue;

            var qty = TryReadInt32(item, "Quantity") ?? 1;
            var amountVat = TryReadDecimal(item, "Amount")
                            ?? TryReadDecimal(item, "Importo");

            result.Add(new AccessoryBoughtLine((short)id.Value, qty, amountVat));
        }

        return result;
    }

    private static List<InsuranceBoughtLine> ExtractInsuranceBought(JsonElement kmNode)
    {
        if ((!TryGetPropertyIgnoreCase(kmNode, "InsurancesBought", out var bought)
             && !TryGetPropertyIgnoreCase(kmNode, "AssicurazioniSelezionateExtra", out bought))
            || bought.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var result = new List<InsuranceBoughtLine>();
        foreach (var item in bought.EnumerateArray())
        {
            var id = TryReadInt32(item, "InsuranceExtraID")
                     ?? TryReadInt32(item, "AssicurazioneID");
            var type = TryReadString(item, "Type", out var t)
                ? t
                : TryReadString(item, "Tipologia", out t) ? t : null;
            var amountVat = TryReadDecimal(item, "Amount")
                            ?? TryReadDecimal(item, "Importo");

            result.Add(new InsuranceBoughtLine(id, type, amountVat));
        }

        return result;
    }

    private static decimal ParseLegacyDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0m;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedInvariant))
            return parsedInvariant;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("it-IT"), out var parsedIt))
            return parsedIt;

        return 0m;
    }

    private static string FormatLegacyDecimal(decimal value)
        => value.ToString("0.00", CultureInfo.InvariantCulture);

    private static bool ValidateRequestedDiscountCode(
        string discountCode,
        string? voucherCliente,
        List<AmountEstimateDiscountDto>? discounts,
        out string? message)
    {
        message = null;
        var requested = discountCode.Trim();
        if (requested.Length == 0)
            return true;

        if (!string.IsNullOrWhiteSpace(voucherCliente)
            && !string.Equals(requested, voucherCliente, StringComparison.OrdinalIgnoreCase))
        {
            message = "Discount code not valid";
            return false;
        }

        if (discounts is not { Count: > 0 })
            return true;

        var matchingDiscount = discounts.FirstOrDefault(d =>
            !string.IsNullOrWhiteSpace(d.HDN_SCN_KEY)
            && string.Equals(d.HDN_SCN_KEY, requested, StringComparison.OrdinalIgnoreCase));

        if (matchingDiscount is null)
        {
            message = "Discount code not valid";
            return false;
        }

        if (matchingDiscount.DiscountIdError.HasValue && matchingDiscount.DiscountIdError.Value != 0)
        {
            message = string.IsNullOrWhiteSpace(matchingDiscount.DiscountMessageError)
                ? "Discount code not valid"
                : matchingDiscount.DiscountMessageError;
            return false;
        }

        return true;
    }

    private sealed record ParsedAmountEstimate(
        AmountEstimateValueDto Amounts,
        AmountEstimateValueDto AmountsWithoutDiscount,
        List<AmountEstimateDiscountDto>? Discount,
        List<AccessoryBoughtLine> AccessoriesBought,
        List<InsuranceBoughtLine> InsurancesBought)
    {
        public AmountEstimateDto ToDto() => new()
        {
            Amounts = Amounts,
            AmountsWithoutDiscount = AmountsWithoutDiscount,
            Discount = Discount
        };
    }

    private sealed record AccessoryBoughtLine(short AccessoryId, int Quantity, decimal? AmountVat);

    private sealed record InsuranceBoughtLine(int? InsuranceExtraId, string? Type, decimal? AmountVat);

    private static bool TryReadDecimalString(JsonElement element, string propertyName, out string value)
    {
        value = string.Empty;
        if (!TryGetPropertyIgnoreCase(element, propertyName, out var property))
            return false;

        if (property.ValueKind == JsonValueKind.String)
        {
            var raw = property.GetString();
            if (string.IsNullOrWhiteSpace(raw))
                return false;

            value = raw;
            return true;
        }

        if (property.ValueKind == JsonValueKind.Number && property.TryGetDecimal(out var numeric))
        {
            value = numeric.ToString("0.00", CultureInfo.InvariantCulture);
            return true;
        }

        return false;
    }

    private static decimal? TryReadDecimal(JsonElement element, string propertyName)
    {
        if (!TryGetPropertyIgnoreCase(element, propertyName, out var property))
            return null;

        if (property.ValueKind == JsonValueKind.Number && property.TryGetDecimal(out var n))
            return n;

        if (property.ValueKind == JsonValueKind.String
            && decimal.TryParse(property.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
            return parsed;

        return null;
    }

    private static int? TryReadInt32(JsonElement element, string propertyName)
    {
        if (!TryGetPropertyIgnoreCase(element, propertyName, out var property))
            return null;

        if (property.ValueKind == JsonValueKind.Number && property.TryGetInt32(out var n))
            return n;

        if (property.ValueKind == JsonValueKind.String && int.TryParse(property.GetString(), out var parsed))
            return parsed;

        return null;
    }

    private static bool TryReadString(JsonElement element, string propertyName, out string value)
    {
        value = string.Empty;
        if (!TryGetPropertyIgnoreCase(element, propertyName, out var property)
            || property.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        value = property.GetString() ?? string.Empty;
        return !string.IsNullOrWhiteSpace(value);
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement element, string propertyName, out JsonElement property)
    {
        if (element.TryGetProperty(propertyName, out property))
            return true;

        foreach (var candidate in element.EnumerateObject())
        {
            if (string.Equals(candidate.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                property = candidate.Value;
                return true;
            }
        }

        property = default;
        return false;
    }

    private sealed class ObjectDynParamDto
    {
        public Dictionary<string, int>? KmType { get; init; }
        public Dictionary<string, string>? StateSegment { get; init; }
        public Dictionary<string, decimal>? ScontoSegment { get; init; }
        public Dictionary<string, List<short>>? Accessory { get; init; }
        public Dictionary<string, List<short>>? AccessoryPreSelected { get; init; }
        public List<string>? Insurance { get; init; }
        public bool Val { get; init; }
        public short MinMezziStart { get; init; }
    }
}
