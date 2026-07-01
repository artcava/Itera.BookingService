using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Estimate;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Application.Estimate;

public sealed class LegacyEstimateService(
    IValidator<WsGetAllCategorieRequest>  getAllCategorieValidator,
    IValidator<WsGetKmsRequest>           getKmsValidator,
    IValidator<WsGetDefaultValuesRequest> getDefaultValuesValidator,
    IValidator<WsGetProvinceRequest>      getProvinceValidator,
    IKmQueryService                       kmQueryService,
    IDurationService                      durationService,
    IProvinceQueryService                 provinceQueryService,
    ILogger<LegacyEstimateService>        logger) : ILegacyEstimateService
{
    private const short BrandScnd = 2;
    private const string DateFormat = "yyyy-MM-ddTHH:mm:ss";

    // ------------------------------------------------------------------
    // GetAllCategory
    // ------------------------------------------------------------------

    public async Task<WsResponse<List<WsCategoria>>> GetAllCategoryAsync(
        WsGetAllCategorieRequest request,
        LegacyAuthContext        authContext,
        CancellationToken        cancellationToken)
    {
        var validation = await getAllCategorieValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return new WsResponse<List<WsCategoria>>
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

        return WsResponse<List<WsCategoria>>.Ok(lista);
    }

    // ------------------------------------------------------------------
    // GetKms
    // ------------------------------------------------------------------

    public async Task<WsResponse<List<WsKmOpzione>>> GetKmsAsync(
        WsGetKmsRequest   request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken)
    {
        var validation = await getKmsValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return new WsResponse<List<WsKmOpzione>>
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

        return WsResponse<List<WsKmOpzione>>.Ok(opzioni);
    }

    // ------------------------------------------------------------------
    // GetDefaultValues
    // ------------------------------------------------------------------

    public async Task<WsResponse<WsGetDefaultValues>> GetDefaultValuesAsync(
        WsGetDefaultValuesRequest request,
        LegacyAuthContext         authContext,
        CancellationToken         ct)
    {
        var validation = await getDefaultValuesValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return new WsResponse<WsGetDefaultValues>
            {
                Esito        = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio    = validation.Errors.First().ErrorMessage
            };

        var dataFrom = DateTime.Today.AddDays(1);
        var dataTo   = DateTime.Today.AddDays(2);

        var result = new WsGetDefaultValues
        {
            DateFromFormatted = dataFrom.ToString("dd/MM/yyyy"),
            DateToFormatted   = dataTo.ToString("dd/MM/yyyy"),
            CategoryID        = WsCategoria.Furgone
        };

        logger.LogInformation(
            "GetDefaultValues: BrandId={BrandId} BranchId={BranchId} DataFrom={DataFrom} DataTo={DataTo} CategoryID={CategoryID}",
            authContext.BrandId, request.BranchID, dataFrom, dataTo, result.CategoryID);

        return WsResponse<WsGetDefaultValues>.Ok(result);
    }

    // ------------------------------------------------------------------
    // GetProvince
    // ------------------------------------------------------------------

    public async Task<WsResponse<List<WsGetProvince>>> GetProvinceAsync(
        WsGetProvinceRequest request,
        LegacyAuthContext    authContext,
        CancellationToken    ct)
    {
        var validation = await getProvinceValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return new WsResponse<List<WsGetProvince>>
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

        return WsResponse<List<WsGetProvince>>.Ok(province);
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

    private static List<WsCategoria> BuildCategorie(int linguaId, short brandId)
    {
        var lista = linguaId == 2
            ? new List<WsCategoria>
              {
                  new() { CategoryID = WsCategoria.Auto,    Description = "Car" },
                  new() { CategoryID = WsCategoria.Furgone, Description = "Van" },
                  new() { CategoryID = WsCategoria.Frigo,   Description = "Refrigerated Van" },
                  new() { CategoryID = WsCategoria.Mobility,Description = "Mobility" },
              }
            : new List<WsCategoria>
              {
                  new() { CategoryID = WsCategoria.Auto,    Description = "Auto" },
                  new() { CategoryID = WsCategoria.Furgone, Description = "Furgoni" },
                  new() { CategoryID = WsCategoria.Frigo,   Description = "Frigo" },
                  new() { CategoryID = WsCategoria.Mobility,Description = "Mobility" },
              };

        if (brandId == BrandScnd)
            lista.Add(linguaId == 2
                ? new() { CategoryID = WsCategoria.FurgoneFrigo, Description = "Van + Refrigerated Van" }
                : new() { CategoryID = WsCategoria.FurgoneFrigo, Description = "Furgoni + Frigo" });

        return lista;
    }
}
