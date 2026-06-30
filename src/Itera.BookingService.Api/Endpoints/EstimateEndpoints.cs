using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Estimate;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class EstimateEndpoints
{
    public static IEndpointRouteBuilder MapEstimateEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/EstimateService.svc").WithTags("EstimateService");

        group.MapPost("/GetAllCategory", async (
            [FromBody] WsGetAllCategorieRequest request,
            HttpContext httpContext,
            ILegacyEstimateService estimateService,
            CancellationToken cancellationToken) =>
        {
            var authContext = (LegacyAuthContext)httpContext.Items[LegacyAuthContext.ItemKey]!;

            var response = await estimateService.GetAllCategoryAsync(request, authContext, cancellationToken);
            return Results.Json(response);
        })
        .WithName("EstimateService_GetAllCategory")
        .WithSummary("Get all vehicle categories")
        .WithDescription("Restituisce le categorie di veicolo disponibili localizzate per lingua e filtrate per brand. Logica puramente in-memory, porting da WsPreventivoBL.GetAllCategorie.")
        .Produces<WsResponse<List<WsCategoria>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        return app;
    }
}
