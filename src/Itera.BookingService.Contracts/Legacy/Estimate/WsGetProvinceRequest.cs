namespace Itera.BookingService.Contracts.Legacy.Estimate;

public class WsGetProvinceRequest : ILegacyTokenCarrier
{
    public string? Token    { get; set; }
    public string? Language { get; set; }
}
