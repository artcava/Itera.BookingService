using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Abstractions;

public interface IEstimateInsuranceQueryService
{
    Task<List<InsuranceExtraDto>> GetInsuranceExtraAsync(
        string segmentCode,
        DateTime dateFrom,
        DateTime dateTo,
        int rentalDays,
        int catalogId,
        CancellationToken cancellationToken);
}