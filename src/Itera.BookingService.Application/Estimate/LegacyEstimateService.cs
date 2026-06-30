using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Estimate;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Application.Estimate;

public sealed class LegacyEstimateService(
    IValidator<WsGetAllCategorieRequest> getAllCategorieValidator,
    ILogger<LegacyEstimateService> logger) : ILegacyEstimateService
{
    // BrandID corrispondente a BrandBL.Brand.SCND nel legacy.
    private const short BrandScnd = 2;

    public async Task<WsResponse<List<WsCategoria>>> GetAllCategoryAsync(
        WsGetAllCategorieRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken)
    {
        var validation = await getAllCategorieValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return new WsResponse<List<WsCategoria>>
            {
                Esito        = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio    = validation.Errors.First().ErrorMessage,
                Data         = []
            };
        }

        var linguaId = ResolveLinguaId(request.Language);
        var lista    = BuildCategorie(linguaId, authContext.BrandId);

        logger.LogInformation(
            "GetAllCategory restituisce {Count} categorie per BrandId {BrandId} LinguaId {LinguaId}",
            lista.Count, authContext.BrandId, linguaId);

        return WsResponse<List<WsCategoria>>.Ok(lista);
    }

    // ---------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------------

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
