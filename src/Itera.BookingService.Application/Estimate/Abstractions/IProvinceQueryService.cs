using Itera.BookingService.Contracts.Legacy.Estimate;

namespace Itera.BookingService.Application.Estimate.Abstractions;

public interface IProvinceQueryService
{
    Task<List<WsGetProvince>> GetProvinceAsync(CancellationToken ct = default);
}
