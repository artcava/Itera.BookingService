using Itera.BookingService.Contracts.General;
using Itera.BookingService.Contracts.Vehicle;

namespace Itera.BookingService.Application.Abstractions;

public interface IVehicleService
{
    Task<ApiResponse<List<MezzoSegmento>>> GetVehicleAsync(
        GetMezziRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken);
}
