using System.Text;
using System.Text.Json;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Branch;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class LegacyEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapLegacyServiceEndpoints(this IEndpointRouteBuilder app)
    {
        MapBranchEndpoints(app);
        //MapClientEndpoints(app);
        MapEstimateEndpoints(app);
        MapSecurityEndpoints(app);
        MapVehicleEndpoints(app);
        //MapMultaEndpoints(app);

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
                return Results.Json(new Contracts.Legacy.WsResponse<object?>
                {
                    Esito = false,
                    CodiceErrore = Contracts.Legacy.LegacyErrorCodes.InvalidToken.ToString(),
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
                return Results.Json(new Contracts.Legacy.WsResponse<object?>
                {
                    Esito = false,
                    CodiceErrore = Contracts.Legacy.LegacyErrorCodes.InvalidToken.ToString(),
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

    private static void MapClientEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/ClientService.svc").WithTags("ClientService");

        MapJsonEndpoint(group, "ClientService", "GetClientInfo", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "GetTypeLicense", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "GetTypeClient", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "Login", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "Logout", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "GetDriverClient", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "AddDriver", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "GetAccountListRentalSoftware360", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "AddAccountRentalSoftware360", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "UpdateAccountRentalSoftware360", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "GetRoleAccountRentalSoftware360", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "GetStateAccountRentalSoftware360", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "GetAccountDetailRentalSoftware360", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "IsAccountRental360LoggedIn", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "DeleteAccountRentalSoftware360", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "ChangePasswordAccountRental360", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "ForceNewPasswordAccountRentalSoftware360", requiresToken: true);
        MapJsonEndpoint(group, "ClientService", "GetPasswordParameters", requiresToken: true);
    }

    private static void MapEstimateEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/EstimateService.svc").WithTags("EstimateService");

        MapJsonEndpoint(group, "EstimateService", "GetAllCategory", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetKms", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetEstimate", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "EstimateConfirmation", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetDefaultValues", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetProvince", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetAccessoryBooking", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetAccessoryBookingFromEstimate", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetNation", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetInsuranceExtra", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetInsuranceExtraFromEstimate", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetAmountEstimate", requiresToken: true);
        MapJsonEndpoint(group, "EstimateService", "GetWholeEstimate", requiresToken: true);
    }

    private static void MapSecurityEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/SecurityService.svc").WithTags("SecurityService");

        MapJsonEndpoint(group, "SecurityService", "GetToken", requiresToken: false);
        MapJsonEndpoint(group, "SecurityService", "ValidateToken", requiresToken: false);
        MapJsonEndpoint(group, "SecurityService", "ResetKeyCache", requiresToken: false);
    }

    private static void MapVehicleEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/VehicleService.svc").WithTags("VehicleService");
        MapJsonEndpoint(group, "VehicleService", "GetVehicle", requiresToken: true);
    }

    private static void MapMultaEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/MultaService.svc").WithTags("MultaService");

        MapXmlEndpoint(group, "MultaService", "NotificaLavorato");
        MapXmlEndpoint(group, "MultaService", "ConfermaLavorato");
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
        {
            endpoint.RequireLegacyToken();
        }
    }

    private static void MapXmlEndpoint(RouteGroupBuilder group, string serviceName, string endpointName)
    {
        group.MapPost($"/{endpointName}", async (
            HttpRequest request,
            ILegacyEndpointExecutor executor,
            CancellationToken cancellationToken) =>
        {
            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var payload = await reader.ReadToEndAsync(cancellationToken);
            var responseXml = await executor.ExecuteXmlAsync(serviceName, endpointName, payload, cancellationToken);
            return Results.Content(responseXml, "application/xml", Encoding.UTF8);
        }).WithName($"{serviceName}_{endpointName}");
    }
}
