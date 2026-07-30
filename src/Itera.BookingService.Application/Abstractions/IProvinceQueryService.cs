using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Abstractions;

public interface IProvinceQueryService
{
    Task<List<GetProvince>> GetProvinceAsync(CancellationToken ct = default);
}
