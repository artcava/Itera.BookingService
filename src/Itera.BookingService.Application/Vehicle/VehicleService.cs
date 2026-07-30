using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Helpers;
using Itera.BookingService.Contracts.General;
using Itera.BookingService.Contracts.Vehicle;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Application.Vehicle;

public sealed class VehicleService(
    IValidator<GetMezziRequest> validator,
    IVehicleQueryService vehicleQueryService,
    ILogger<VehicleService> logger) : IVehicleService
{
    public async Task<ApiResponse<List<MezzoSegmento>>> GetVehicleAsync(
        GetMezziRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return ResponseHelper.LegacyError<List<MezzoSegmento>>("VALIDATION_ERROR", validation.Errors.First().ErrorMessage);
        }

        var result = await vehicleQueryService.GetMezziAsync(
            request.FleetMulti,
            request.SegmentoMulti,
            request.MezzoSpeciale,
            request.GruppoID,
            cancellationToken);

        logger.LogInformation(
            "GetVehicle resolved {Count} mezzi for WsUserID {WsUserID}",
            result.Count, authContext.WsUserId);

        return ApiResponse<List<MezzoSegmento>>.Ok(result);
    }
}
