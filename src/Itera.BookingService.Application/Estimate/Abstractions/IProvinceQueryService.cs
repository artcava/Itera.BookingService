using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Abstractions;

public interface IProvinceQueryService
{
    Task<List<GetProvince>> GetProvinceAsync(CancellationToken ct = default);
}
