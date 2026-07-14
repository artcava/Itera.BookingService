using Itera.BookingService.Contracts.Abstractions;

namespace Itera.BookingService.Contracts.Estimate;

public sealed record GetInsuranceExtraRequest : BaseRequest, ILegacyTokenCarrier
{
    public string? Token { get; set; }
    public required string SegmentCode { get; init; }
    public int RentalDays { get; init; }
    public int CatalogId { get; init; }
    public string DateFrom { get; init; } = string.Empty;
    public string DateTo { get; init; } = string.Empty;
}
