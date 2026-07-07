using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Abstractions;

public interface IEstimateAccessoryQueryService
{
    Task<List<AccessoryBookingDto>> GetAccessoryBookingAsync(
        short brandId,
        int branchId,
        int branchDestinationId,
        int catalogId,
        int rentalDays,
        DateTime dateFrom,
        DateTime dateTo,
        string? categoryId,
        string? segmentCode,
        CancellationToken cancellationToken);
}