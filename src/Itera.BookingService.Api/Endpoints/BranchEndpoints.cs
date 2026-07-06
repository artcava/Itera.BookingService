using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Branch;
using Microsoft.AspNetCore.Mvc;
using Itera.BookingService.Contracts.General;

namespace Itera.BookingService.Api.Endpoints;

public static class BranchEndpoints
{
    public static IEndpointRouteBuilder MapBranchEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/BranchService.svc").WithTags("BranchService");

        group.MapPost("/GetAllBranches", async (
            [FromBody] GetAllBranchesRequest request,
            HttpContext httpContext,
            IBranchService branchService,
            CancellationToken cancellationToken) =>
        {
            var authContext = (LegacyAuthContext)httpContext.Items[LegacyAuthContext.ItemKey]!;

            var response = await branchService.GetAllBranchesAsync(request, authContext, cancellationToken);
            return Results.Json(response);
        })
        .WithName("BranchService_GetAllBranches")
        .WithSummary("Get all branches")
        .WithDescription("Legacy-compatible GetAllBranches endpoint with token validation and language/date parity.")
        .Produces<ApiResponse<List<FilialeDto>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        group.MapPost("/GetInfoBranch", async (
            [FromBody] GetBranchInfoRequest request,
            HttpContext httpContext,
            IBranchService branchService,
            CancellationToken cancellationToken) =>
        {
            var authContext = (LegacyAuthContext)httpContext.Items[LegacyAuthContext.ItemKey]!;

            var response = await branchService.GetInfoBranchAsync(request, authContext, cancellationToken);
            return Results.Json(response);
        })
        .WithName("BranchService_GetInfoBranch")
        .WithSummary("Get branch details")
        .WithDescription("Legacy-compatible GetInfoBranch endpoint with token validation and language/date parity.")
        .Produces<ApiResponse<FilialeDto?>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        return app;
    }
}
