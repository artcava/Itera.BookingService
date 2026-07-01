using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Estimate;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Application.Estimate;

public sealed class LegacyEstimateService(
    IValidator<WsGetAllCategorieRequest> getAllCategorieValidator,
    IValidator<WsGetKmsRequest>          getKmsValidator,
    IKmQueryService                      kmQueryService,
    IDurationService                     durationService,
    ILogger<LegacyEstimateService>       logger) : ILegacyEstimateService
{
    // BrandID corrispondente a BrandBL.Brand.SCND nel legacy.
    private const short BrandScnd = 2;

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
            return WsResponse<List<WsCategoria>>.ValidationError(
                validation.Errors.First().ErrorMessage);

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
            return WsResponse<List<WsKmOpzione>>.ValidationError(
                validation.Errors.First().ErrorMessage);

        var dataFrom = ParseDate(request.DataFrom);
        var dataTo   = ParseDate(request.DataTo);

        // Calcolo durata: replica DurataBL.CalcolaDurata24HByDate del legacy.
        // Se il periodo supera la soglia mensile, DurationService restituisce
        // NewDataTo != null ("PeriodoSuperioreAlMese") — in quel caso il legacy
        // tronca la finestra a un mese e va a cercare il listino mensile (codice "M").
        // Usiamo NewDataTo per la query EF in modo da ottenere le opzioni km
        // del listino corretto per la durata effettiva normalizzata.
        var durata         = durationService.Calcola(dataFrom, dataTo);
        var dataToEffettiva = durata.NewDataTo ?? dataTo;

        if (durata.NewDataTo is not null)
            logger.LogInformation(
                "GetKms: PeriodoSuperioreAlMese rilevato. DataTo originale={DataToOriginale} normalizzata a {DataToNormalizzata}",
                dataTo, dataToEffettiva);

        var opzioni = await kmQueryService.GetKmsAsync(
            filialeId:            request.FilialeId,
            categoriaId:          request.CategoriaId,
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
    // Helpers privati
    // ------------------------------------------------------------------

    private static DateTime ParseDate(string value) =>
        DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss",
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
        {
            lista.Add(linguaId == 2
                ? new() { CategoryID = WsCategoria.FurgoneFrigo, Description = "Van + Refrigerated Van" }
                : new() { CategoryID = WsCategoria.FurgoneFrigo, Description = "Furgoni + Frigo" });
        }

        return lista;
    }
}
