using System.Text.Json;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Branch;
using Itera.BookingService.Contracts.Legacy.Vehicle;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class LegacyEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapLegacyServiceEndpoints(this IEndpointRouteBuilder app)
    {
        MapBranchEndpoints(app);
        MapEstimateEndpoints(app);
        MapVehicleEndpoints(app);
        // Security endpoints registrati separatamente in Program.cs via MapSecurityEndpoints()

        return app;
    }

    private static void MapBranchEndpoints(IEndpointRouteBuilder app)
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
    }

    private static void MapEstimateEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/EstimateService.svc").WithTags("EstimateService");

        MapJsonEndpoint(group, "EstimateService", "GetAllCategory",                   requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetKms",                           requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetEstimate",                      requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "EstimateConfirmation",             requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetDefaultValues",                requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetProvince",                     requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetAccessoryBooking",             requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetAccessoryBookingFromEstimate", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetNation",                       requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetInsuranceExtra",               requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetInsuranceExtraFromEstimate",   requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetAmountEstimate",               requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetWholeEstimate",                requiresToken: true);
    }

    private static void MapVehicleEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/VehicleService.svc").WithTags("VehicleService");

        group.MapPost("/GetVehicle", async (
            [FromBody] WsGetMezziRequest request,
            HttpContext httpContext,
            ILegacyVehicleService vehicleService,
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

            var response = await vehicleService.GetVehicleAsync(request, authContext, cancellationToken);
            return Results.Json(response);
        })
        .WithName("VehicleService_GetVehicle")
        .WithSummary("Get vehicles by segment")
        .WithDescription("Legacy-compatible GetVehicle endpoint. Supports optional filters: FleetMulti (CSV), SegmentoMulti (CSV), MezzoSpeciale, GruppoID.")
        .Produces<WsResponse<List<WsMezzoSegmento>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();
    }

    private static void MapJsonEndpoint(RouteGroupBuilder group, string serviceName, string endpointName, bool requiresToken)
    {
        var endpoint = group.MapPost($"/{endpointName}", async (
            [FromBody] JsonElement payload,
            ILegacyEndpointExecutor executor,
            CancellationToken cancellationToken) =>
        {
            var response = await executor.ExecuteJsonAsync(serviceName, endpointName, payload, cancellationToken);
            return Results.Json(response);
        }).WithName($"{serviceName}_{endpointName}");

        if (requiresToken)
            endpoint.RequireLegacyToken();
    }
}
