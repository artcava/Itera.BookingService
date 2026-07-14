using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Infrastructure.Persistence;

namespace Itera.BookingService.Infrastructure.Estimate;

internal sealed class EstimateInsuranceQueryService(LegacyDbContext dbContext) : IEstimateInsuranceQueryService
{
    public async Task<List<InsuranceExtraDto>> GetInsuranceExtraAsync(
        string segmentCode,
        DateTime dateFrom,
        DateTime dateTo,
        int rentalDays,
        int catalogId,
        CancellationToken cancellationToken)
    {
        return await estimateInsuranceQueryRepository.GetInsuranceExtraAsync(
            segmentCode,
            dateFrom,
            dateTo,
            rentalDays,
            catalogId,
            cancellationToken);
    }
}