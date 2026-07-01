using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Vehicle;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class VehicleEndpoints
{
    public static IEndpointRouteBuilder MapVehicleEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/VehicleService.svc").WithTags("VehicleService");

        group.MapPost("/GetVehicle", async (
            [FromBody] GetMezziRequest request,
            HttpContext httpContext,
            ILegacyVehicleService vehicleService,
            CancellationToken cancellationToken) =>
        {
            if (!httpContext.Items.TryGetValue(LegacyAuthContext.ItemKey, out var authContextRaw)
                || authContextRaw is not LegacyAuthContext authContext)
            {
                return Results.Json(new ApiResponse<object?>
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
        .Produces<ApiResponse<List<MezzoSegmento>>>(StatusCodes.Status200OK)
        .RequireLegacyToken();

        return app;
    }
}
