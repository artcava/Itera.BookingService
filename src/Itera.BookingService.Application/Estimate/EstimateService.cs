using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Contracts.General;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Application.Estimate;

public sealed class EstimateService(
    IValidator<GetAllCategorieRequest>      getAllCategorieValidator,
    IValidator<GetKmsRequest>               getKmsValidator,
    IValidator<GetDefaultValuesRequest>     getDefaultValuesValidator,
    IValidator<GetProvinceRequest>          getProvinceValidator,
    IValidator<GetNationsRequest>           getNationsValidator,
    IValidator<GetAccessoryBookingRequest>  getAccessoryBookingValidator,
    IKmQueryService                         kmQueryService,
    IDurationService                        durationService,
    IProvinceQueryService                   provinceQueryService,
    INationQueryService                     nationQueryService,
    IEstimateAccessoryQueryService          estimateAccessoryQueryService,
    ILogger<EstimateService>                logger) : IEstimateService
{
    private const short BrandScnd = 2;
    private const string DateFormat = "yyyy-MM-ddTHH:mm:ss";

    // ------------------------------------------------------------------
    // GetAllCategory
    // ------------------------------------------------------------------

    public async Task<ApiResponse<List<Categoria>>> GetAllCategoryAsync(
        GetAllCategorieRequest request,
        LegacyAuthContext        authContext,
        CancellationToken        cancellationToken)
    {
        var validation = await getAllCategorieValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return new ApiResponse<List<Categoria>>
            {
                Esito        = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio    = validation.Errors.First().ErrorMessage,
                Data         = []
            };

        var linguaId = ResolveLinguaId(request.Language);
        var lista    = BuildCategorie(linguaId, authContext.BrandId);

        logger.LogInformation(
            "GetAllCategory restituisce {Count} categorie per BrandId={BrandId} LinguaId={LinguaId}",
            lista.Count, authContext.BrandId, linguaId);

        return ApiResponse<List<Categoria>>.Ok(lista);
    }

    // ------------------------------------------------------------------
    // GetKms
    // ------------------------------------------------------------------

    public async Task<ApiResponse<List<KmOpzione>>> GetKmsAsync(
        GetKmsRequest     request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken)
    {
        var validation = await getKmsValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return new ApiResponse<List<KmOpzione>>
            {
                Esito        = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio    = validation.Errors.First().ErrorMessage,
                Data         = []
            };

        var dataFrom = ParseDate(request.DataFrom!);
        var dataTo   = ParseDate(request.DataTo!);

        var durata          = durationService.Calcola(dataFrom, dataTo);
        var dataToEffettiva = durata.NewDataTo ?? dataTo;

        if (durata.NewDataTo is not null)
            logger.LogInformation(
                "GetKms: PeriodoSuperioreAlMese. DataTo originale={DataToOriginale} normalizzata a {DataToNormalizzata}",
                dataTo, dataToEffettiva);

        var opzioni = await kmQueryService.GetKmsAsync(
            filialeId:            request.FilialeId,
            categoriaId:          request.CategoriaId!,
            dataFrom:             dataFrom,
            dataTo:               dataToEffettiva,
            fasciaOrarioRitiro:   request.FasciaOrarioRitiro,
            fasciaOrarioConsegna: request.FasciaOrarioConsegna,
            cancellationToken:    cancellationToken);

        logger.LogInformation(
            "GetKms: FilialeId={FilialeId} CategoriaId={CategoriaId} CodiceDurata={CodiceDurata} Giorni={Giorni} Opzioni={Count}",
            request.FilialeId, request.CategoriaId, durata.CodiceDurata, durata.Giorni, opzioni.Count);

        return ApiResponse<List<KmOpzione>>.Ok(opzioni);
    }

    // ------------------------------------------------------------------
    // GetDefaultValues
    // ------------------------------------------------------------------

    public async Task<ApiResponse<GetDefaultValues>> GetDefaultValuesAsync(
        GetDefaultValuesRequest request,
        LegacyAuthContext       authContext,
        CancellationToken       ct)
    {
        var validation = await getDefaultValuesValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return new ApiResponse<GetDefaultValues>
            {
                Esito        = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio    = validation.Errors.First().ErrorMessage
            };

        var dataFrom = DateTime.Today.AddDays(1);
        var dataTo   = DateTime.Today.AddDays(2);

        var result = new GetDefaultValues
        {
            DateFromFormatted = dataFrom.ToString("dd/MM/yyyy"),
            DateToFormatted   = dataTo.ToString("dd/MM/yyyy"),
            CategoryID        = Categoria.Furgone
        };

        logger.LogInformation(
            "GetDefaultValues: BrandId={BrandId} BranchId={BranchId} DataFrom={DataFrom} DataTo={DataTo} CategoryID={CategoryID}",
            authContext.BrandId, request.BranchID, dataFrom, dataTo, result.CategoryID);

        return ApiResponse<GetDefaultValues>.Ok(result);
    }

    // ------------------------------------------------------------------
    // GetProvince
    // ------------------------------------------------------------------

    public async Task<ApiResponse<List<GetProvince>>> GetProvinceAsync(
        GetProvinceRequest request,
        LegacyAuthContext    authContext,
        CancellationToken    ct)
    {
        var validation = await getProvinceValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return new ApiResponse<List<GetProvince>>
            {
                Esito        = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio    = validation.Errors.First().ErrorMessage,
                Data         = []
            };

        var province = await provinceQueryService.GetProvinceAsync(ct);

        logger.LogInformation(
            "GetProvince: restituite {Count} province per BrandId={BrandId}",
            province.Count, authContext.BrandId);

        return ApiResponse<List<GetProvince>>.Ok(province);
    }

    // ------------------------------------------------------------------
    // GetNations
    // ------------------------------------------------------------------

    public async Task<ApiResponse<List<Nazione>>> GetNationsAsync(
        GetNationsRequest request,
        LegacyAuthContext authContext,
        CancellationToken ct)
    {
        var validation = await getNationsValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return new ApiResponse<List<Nazione>>
            {
                Esito        = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio    = validation.Errors.First().ErrorMessage,
                Data         = []
            };

        var nazioni = await nationQueryService.GetNationsAsync(request.Language, ct);

        logger.LogInformation(
            "GetNations: restituite {Count} nazioni per Language={Language}",
            nazioni.Count, request.Language);

        return ApiResponse<List<Nazione>>.Ok(nazioni);
    }

    // ------------------------------------------------------------------
    // GetAccessoryBooking
    // ------------------------------------------------------------------

    public async Task<ApiResponse<List<AccessoryBookingDto>>> GetAccessoryBookingAsync(
        GetAccessoryBookingRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken)
    {
        var validation = await getAccessoryBookingValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return new ApiResponse<List<AccessoryBookingDto>>
            {
                Esito = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio = validation.Errors.First().ErrorMessage,
                Data = []
            };
        }

        var linguaId = LegacyRequestCultureDateResolver.ResolveLinguaId(request.Language);
        var dateFrom = LegacyRequestCultureDateResolver.ResolveDateStartLegacy(request.DateFrom, linguaId);
        var dateTo = LegacyRequestCultureDateResolver.ResolveDateEndLegacy(request.DateTo, linguaId);

        // Recupero accordo commerciale dal contesto
        // (stessa logica del legacy: accordoCommercialeID viene risolto
        //  a monte prima di chiamare il layer Infrastructure)
        var accordoCommercialeId = authContext.AccordoCommercialeId;

        var accessories = await estimateAccessoryQueryService.GetAccessoryBookingAsync(
            authContext.BrandId,
            request.BranchId,
            request.BranchDestinationId,
            request.CatalogId,
            request.RentalDays,
            dateFrom,
            dateTo,
            request.CategoryId,
            request.SegmentCode,
            accordoCommercialeId,
            cancellationToken);

        logger.LogInformation(
            "GetAccessoryBooking resolved {Count} accessories for BranchID {BranchId} WsUserID {WsUserId}",
            accessories.Count,
            request.BranchId,
            authContext.WsUserId);

        return ApiResponse<List<AccessoryBookingDto>>.Ok(accessories);
    }

    // ------------------------------------------------------------------
    // Helpers privati
    // ------------------------------------------------------------------

    private static DateTime ParseDate(string value) =>
        DateTime.ParseExact(value, DateFormat,
            System.Globalization.CultureInfo.InvariantCulture);

    private static int ResolveLinguaId(string? language)
        => string.Equals(language, "en", StringComparison.OrdinalIgnoreCase)
           || string.Equals(language, "2", StringComparison.OrdinalIgnoreCase)
            ? 2
            : 1;

    private static List<Categoria> BuildCategorie(int linguaId, short brandId)
    {
        var lista = linguaId == 2
            ? new List<Categoria>
              {
                  new() { CategoryID = Categoria.Auto,    Description = "Car" },
                  new() { CategoryID = Categoria.Furgone, Description = "Van" },
                  new() { CategoryID = Categoria.Frigo,   Description = "Refrigerated Van" },
                  new() { CategoryID = Categoria.Mobility,Description = "Mobility" },
              }
            : new List<Categoria>
              {
                  new() { CategoryID = Categoria.Auto,    Description = "Auto" },
                  new() { CategoryID = Categoria.Furgone, Description = "Furgoni" },
                  new() { CategoryID = Categoria.Frigo,   Description = "Frigo" },
                  new() { CategoryID = Categoria.Mobility,Description = "Mobility" },
              };

        if (brandId == BrandScnd)
            lista.Add(linguaId == 2
                ? new() { CategoryID = Categoria.FurgoneFrigo, Description = "Van + Refrigerated Van" }
                : new() { CategoryID = Categoria.FurgoneFrigo, Description = "Furgoni + Frigo" });

        return lista;
    }
}
