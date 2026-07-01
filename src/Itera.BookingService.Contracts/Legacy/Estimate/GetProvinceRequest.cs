namespace Itera.BookingService.Contracts.Legacy.Estimate;

public class GetProvinceRequest : ILegacyTokenCarrier
{
    public string? Token    { get; set; }
    public string? Language { get; set; }
}
