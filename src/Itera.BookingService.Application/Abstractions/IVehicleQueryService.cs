using Itera.BookingService.Contracts.Vehicle;

namespace Itera.BookingService.Application.Abstractions;

public interface IVehicleQueryService
{
    Task<List<MezzoSegmento>> GetMezziAsync(
        string? fleetMulti,
        string? segmentoMulti,
        bool? mezzoSpeciale,
        int? gruppoId,
        CancellationToken cancellationToken);
}
