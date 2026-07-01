using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Vehicle;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Application.Vehicle;

public sealed class LegacyVehicleService(
    IValidator<GetMezziRequest> validator,
    IVehicleQueryService vehicleQueryService,
    ILogger<LegacyVehicleService> logger) : ILegacyVehicleService
{
    public async Task<ApiResponse<List<MezzoSegmento>>> GetVehicleAsync(
        GetMezziRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return new ApiResponse<List<MezzoSegmento>>
            {
                Esito = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio = validation.Errors.First().ErrorMessage,
                Data = []
            };
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
