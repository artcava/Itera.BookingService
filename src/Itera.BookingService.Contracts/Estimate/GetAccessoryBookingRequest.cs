using Itera.BookingService.Contracts.Abstractions;

namespace Itera.BookingService.Contracts.Estimate;

public sealed record GetAccessoryBookingRequest : BaseRequest, ILegacyTokenCarrier
{
    public string? Token { get; set; }
    public int BranchId { get; init; }
    public int BranchDestinationId { get; init; }
    public int CatalogId { get; init; }
    public int RentalDays { get; init; }
    public string DateFrom { get; init; } = string.Empty;
    public string DateTo { get; init; } = string.Empty;
    public string? CategoryId { get; init; }
    public string? SegmentCode { get; init; }
}