namespace Itera.BookingService.Contracts.Estimate;

public sealed record GetAccessoryBookingRequest
{
    public short BrandId { get; init; }
    public int BranchId { get; init; }
    public int BranchDestinationId { get; init; }
    public int CatalogId { get; init; }
    public int RentalDays { get; init; }
    public string DateFrom { get; init; } = string.Empty;
    public string DateTo { get; init; } = string.Empty;
    public string? CategoryId { get; init; }
    public string? SegmentCode { get; init; }
    public string? Language { get; init; }
}