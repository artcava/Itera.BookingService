using Itera.BookingService.Contracts.Legacy.Vehicle;

namespace Itera.BookingService.Application.Abstractions;

public interface IVehicleQueryService
{
    Task<List<WsMezzoSegmento>> GetMezziAsync(
        string? fleetMulti,
        string? segmentoMulti,
        bool? mezzoSpeciale,
        int? gruppoId,
        CancellationToken cancellationToken);
}
