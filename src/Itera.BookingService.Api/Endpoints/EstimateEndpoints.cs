using System.Text.Json;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Contracts.General;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class EstimateEndpoints
{
    private static readonly IReadOnlyList<string> _notImplementedMethods =
    [
        "GetEstimate",
        "EstimateConfirmation",
        "GetAccessoryBooking",
        "GetAccessoryBookingFromEstimate",
        "GetNation",
        "GetInsuranceExtra",
        "GetInsuranceExtraFromEstimate",
        "GetAmountEstimate",
        "GetWholeEstimate"
    ];

    public static IEndpointRouteBuilder MapEstimateEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/EstimateService.svc").WithTags("EstimateService");

        // --- Implementati ---

        group.MapPost("/GetAllCategory", async (
            [FromBody] GetAllCategorieRequest request,
            HttpContext httpContext,
            IEstimateService estimateService,
            CancellationToken cancellationToken) =>
        {
            var authContext = (LegacyAuthContext)httpContext.Items[LegacyAuthContext.ItemKey]!;
            return Results.Json(await estimateService.GetAllCategoryAsync(request, authContext, cancellationToken));
        })
        .WithName("EstimateService_GetAllCategory")
        .WithSummary("Get all vehicle categories")
        .WithDescription("Restituisce le categorie di veicolo disponibili localizzate per lingua e filtrate per brand. Logica puramente in-memory, porting da WsPreventivoBL.GetAllCategorie.")
        .Produces<ApiResponse<List<Categoria>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        group.MapPost("/GetKms", async (
            [FromBody] GetKmsRequest request,
            HttpContext httpContext,
            IEstimateService estimateService,
            CancellationToken cancellationToken) =>
        {
            var authContext = (LegacyAuthContext)httpContext.Items[LegacyAuthContext.ItemKey]!;
            return Results.Json(await estimateService.GetKmsAsync(request, authContext, cancellationToken));
        })
        .WithName("EstimateService_GetKms")
        .WithSummary("Get available km options")
        .WithDescription("Restituisce le opzioni km disponibili per filiale, categoria veicolo e finestra temporale. Gestisce PeriodoSuperioreAlMese tronando la finestra a un mese. Porting da WsPreventivoBL.GetKms.")
        .Produces<ApiResponse<List<KmOpzione>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        group.MapPost("/GetDefaultValues", async (
            [FromBody] GetDefaultValuesRequest request,
            HttpContext                          httpContext,
            IEstimateService                     estimateService,
            CancellationToken                    cancellationToken) =>
        {
            var authContext = (LegacyAuthContext)httpContext.Items[LegacyAuthContext.ItemKey]!;
            return Results.Json(
                await estimateService.GetDefaultValuesAsync(request, authContext, cancellationToken));
        })
        .WithName("EstimateService_GetDefaultValues")
        .WithSummary("Get default rental values")
        .WithDescription(
            "Restituisce i valori di default per la ricerca preventivo: " +
            "date di ritiro/consegna (oggi+1/oggi+2) e categoria veicolo predefinita. " +
            "Logica puramente in-memory, porting da WsPreventivoBL.GetDefaultValues. " +
            "Il ramo GetPrimoGiornoUtilePerRitiro \u00e8 disabilitato nel legacy e non portato.")
        .Produces<ApiResponse<GetDefaultValues>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        group.MapPost("/GetProvince", async (
            [FromBody] GetProvinceRequest request,
            HttpContext                     httpContext,
            IEstimateService                estimateService,
            CancellationToken               cancellationToken) =>
        {
            var authContext = (LegacyAuthContext)httpContext.Items[LegacyAuthContext.ItemKey]!;
            return Results.Json(
                await estimateService.GetProvinceAsync(request, authContext, cancellationToken));
        })
        .WithName("EstimateService_GetProvince")
        .WithSummary("Get province list")
        .WithDescription(
            "Restituisce l'elenco delle province ordinate per denominazione. " +
            "Query diretta su tabella Province (entit\u00e0 normale EF Core, nessun keyless type). " +
            "Porting da WsPreventivoBL.GetProvince.")
        .Produces<ApiResponse<List<GetProvince>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        // --- Stub NOT_IMPLEMENTED (da migrare) ---

        foreach (var method in _notImplementedMethods)
        {
            var capturedMethod = method;
            group.MapPost($"/{capturedMethod}", (
                [FromBody] JsonElement _,
                CancellationToken __) =>
            {
                var response = ApiResponse<object?>.NotImplemented($"EstimateService/{capturedMethod}");
                return Results.Json(response);
            })
            .WithName($"EstimateService_{capturedMethod}")
            .RequireLegacyToken();
        }

        return app;
    }
}
