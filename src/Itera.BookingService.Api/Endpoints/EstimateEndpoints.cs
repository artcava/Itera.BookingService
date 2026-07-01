using System.Text.Json;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Estimate;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class EstimateEndpoints
{
    private static readonly IReadOnlyList<string> _notImplementedMethods =
    [
        "GetEstimate",
        "EstimateConfirmation",
        "GetDefaultValues",
        "GetProvince",
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
            [FromBody] WsGetAllCategorieRequest request,
            HttpContext httpContext,
            ILegacyEstimateService estimateService,
            CancellationToken cancellationToken) =>
        {
            var authContext = (LegacyAuthContext)httpContext.Items[LegacyAuthContext.ItemKey]!;
            return Results.Json(await estimateService.GetAllCategoryAsync(request, authContext, cancellationToken));
        })
        .WithName("EstimateService_GetAllCategory")
        .WithSummary("Get all vehicle categories")
        .WithDescription("Restituisce le categorie di veicolo disponibili localizzate per lingua e filtrate per brand. Logica puramente in-memory, porting da WsPreventivoBL.GetAllCategorie.")
        .Produces<WsResponse<List<WsCategoria>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        group.MapPost("/GetKms", async (
            [FromBody] WsGetKmsRequest request,
            HttpContext httpContext,
            ILegacyEstimateService estimateService,
            CancellationToken cancellationToken) =>
        {
            var authContext = (LegacyAuthContext)httpContext.Items[LegacyAuthContext.ItemKey]!;
            return Results.Json(await estimateService.GetKmsAsync(request, authContext, cancellationToken));
        })
        .WithName("EstimateService_GetKms")
        .WithSummary("Get available km options")
        .WithDescription("Restituisce le opzioni km disponibili per filiale, categoria veicolo e finestra temporale. Gestisce PeriodoSuperioreAlMese tronando la finestra a un mese. Porting da WsPreventivoBL.GetKms.")
        .Produces<WsResponse<List<WsKmOpzione>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        // --- Stub NOT_IMPLEMENTED (da migrare) ---

        foreach (var method in _notImplementedMethods)
        {
            var capturedMethod = method;
            group.MapPost($"/{capturedMethod}", async (
                [FromBody] JsonElement payload,
                ILegacyEndpointExecutor executor,
                CancellationToken cancellationToken) =>
            {
                var response = await executor.ExecuteJsonAsync("EstimateService", capturedMethod, payload, cancellationToken);
                return Results.Json(response);
            })
            .WithName($"EstimateService_{capturedMethod}")
            .RequireLegacyToken();
        }

        return app;
    }
}
