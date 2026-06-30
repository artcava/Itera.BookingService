using System.Text.Json;
using Itera.BookingService.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class EstimateEndpoints
{
    private static readonly IReadOnlyList<string> _methods =
    [
        "GetAllCategory",
        "GetKms",
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

        foreach (var method in _methods)
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
