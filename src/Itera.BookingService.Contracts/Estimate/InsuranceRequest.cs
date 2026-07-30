namespace Itera.BookingService.Contracts.Estimate;

public sealed record InsuranceRequest
{
    public string Type { get; init; } = string.Empty;
}
