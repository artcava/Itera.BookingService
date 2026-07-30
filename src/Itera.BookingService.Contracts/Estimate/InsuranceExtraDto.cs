namespace Itera.BookingService.Contracts.Estimate;

public sealed record InsuranceExtraDto
{
    public int InsuranceExtraID { get; init; }
    public string? InsuranceExtraDescr { get; init; }
    public string? InsuranceExtra { get; init; }
    public string? InsuranceExtraWithoutIVA { get; init; }
    public string? Type { get; set; }
    public string? CategoryID { get; set; }
}