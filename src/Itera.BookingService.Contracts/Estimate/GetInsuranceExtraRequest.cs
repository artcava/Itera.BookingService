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

public sealed record GetAmountEstimateRequest : BaseRequest, ILegacyTokenCarrier
{
    public string? Token { get; set; }
    public string EstimateToken { get; init; } = string.Empty;
    public string SegmentCode { get; init; } = string.Empty;
    public string KmType { get; init; } = string.Empty;
    public List<int>? InsuranceExtraList { get; init; }
    public List<AmountEstimateInsuranceRequest>? InsuranceList { get; init; }
    public List<AmountEstimateAccessoryRequest>? AccessoryList { get; init; }
    public string? DiscountCode { get; init; }
}

public sealed record AmountEstimateInsuranceRequest
{
    public string Type { get; init; } = string.Empty;
}

public sealed record AmountEstimateAccessoryRequest
{
    public short AccessoryID { get; init; }
    public short Quantity { get; init; }
}
