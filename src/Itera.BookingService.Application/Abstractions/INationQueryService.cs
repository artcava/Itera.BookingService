using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Abstractions;

public interface INationQueryService
{
    Task<List<Nazione>> GetNationsAsync(string? language, CancellationToken ct = default);
}