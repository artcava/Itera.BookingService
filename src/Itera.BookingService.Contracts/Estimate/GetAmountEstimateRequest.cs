using Itera.BookingService.Contracts.Abstractions;

namespace Itera.BookingService.Contracts.Estimate;

public sealed record GetAmountEstimateRequest : BaseRequest, ILegacyTokenCarrier
{
    public string? Token { get; set; }
    public string EstimateToken { get; init; } = string.Empty;
    public string SegmentCode { get; init; } = string.Empty;
    public string KmType { get; init; } = string.Empty;
    public List<int>? InsuranceExtraList { get; init; }
    public List<InsuranceRequest>? InsuranceList { get; init; }
    public List<AccessoryRequest>? AccessoryList { get; init; }
    public string? DiscountCode { get; init; }
}


