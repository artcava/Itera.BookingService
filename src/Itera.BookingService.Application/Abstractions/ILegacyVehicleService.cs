using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Vehicle;

namespace Itera.BookingService.Application.Abstractions;

public interface ILegacyVehicleService
{
    Task<WsResponse<List<WsMezzoSegmento>>> GetVehicleAsync(
        WsGetMezziRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken);
}
