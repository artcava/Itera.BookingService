using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Branch;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class BranchEndpoints
{
    public static IEndpointRouteBuilder MapBranchEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/BranchService.svc").WithTags("BranchService");

        group.MapPost("/GetAllBranches", async (
            [FromBody] WsGetAllFilialiRequest request,
            HttpContext httpContext,
            ILegacyBranchService branchService,
            CancellationToken cancellationToken) =>
        {
            if (!httpContext.Items.TryGetValue(LegacyAuthContext.ItemKey, out var authContextRaw)
                || authContextRaw is not LegacyAuthContext authContext)
            {
                return Results.Json(new WsResponse<object?>
                {
                    Esito = false,
                    CodiceErrore = LegacyErrorCodes.InvalidToken.ToString(),
                    Messaggio = "Invalid token",
                    Data = null
                });
            }

            var response = await branchService.GetAllBranchesAsync(request, authContext, cancellationToken);
            return Results.Json(response);
        })
        .WithName("BranchService_GetAllBranches")
        .WithSummary("Get all branches")
        .WithDescription("Legacy-compatible GetAllBranches endpoint with token validation and language/date parity.")
        .Produces<WsResponse<List<WsFiliale>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        group.MapPost("/GetInfoBranch", async (
            [FromBody] WsGetFilialeInfoRequest request,
            HttpContext httpContext,
            ILegacyBranchService branchService,
            CancellationToken cancellationToken) =>
        {
            if (!httpContext.Items.TryGetValue(LegacyAuthContext.ItemKey, out var authContextRaw)
                || authContextRaw is not LegacyAuthContext authContext)
            {
                return Results.Json(new WsResponse<object?>
                {
                    Esito = false,
                    CodiceErrore = LegacyErrorCodes.InvalidToken.ToString(),
                    Messaggio = "Invalid token",
                    Data = null
                });
            }

            var response = await branchService.GetInfoBranchAsync(request, authContext, cancellationToken);
            return Results.Json(response);
        })
        .WithName("BranchService_GetInfoBranch")
        .WithSummary("Get branch details")
        .WithDescription("Legacy-compatible GetInfoBranch endpoint with token validation and language/date parity.")
        .Produces<WsResponse<WsFiliale?>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        return app;
    }
}
