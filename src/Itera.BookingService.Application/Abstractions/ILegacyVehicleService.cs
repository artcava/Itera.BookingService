using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Vehicle;

namespace Itera.BookingService.Application.Abstractions;

public interface ILegacyVehicleService
{
    Task<ApiResponse<List<MezzoSegmento>>> GetVehicleAsync(
        GetMezziRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken);
}
