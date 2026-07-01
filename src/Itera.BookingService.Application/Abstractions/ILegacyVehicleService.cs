using Itera.BookingService.Contracts.General;
using Itera.BookingService.Contracts.Vehicle;

namespace Itera.BookingService.Application.Abstractions;

public interface ILegacyVehicleService
{
    Task<ApiResponse<List<MezzoSegmento>>> GetVehicleAsync(
        GetMezziRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken);
}
