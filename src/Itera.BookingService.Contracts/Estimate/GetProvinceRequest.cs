using Itera.BookingService.Contracts.Abstractions;

namespace Itera.BookingService.Contracts.Estimate;

public class GetProvinceRequest : ILegacyTokenCarrier
{
    public string? Token    { get; set; }
    public string? Language { get; set; }
}
