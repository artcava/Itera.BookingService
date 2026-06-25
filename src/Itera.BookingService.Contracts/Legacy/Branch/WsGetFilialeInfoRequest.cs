namespace Itera.BookingService.Contracts.Legacy.Branch;

public class WsGetFilialeInfoRequest : ILegacyTokenCarrier
{
    public string? Token { get; set; }
    public string? Language { get; set; }
    public string? DateStart { get; set; }
    public int BranchID { get; set; }
    public bool? GetFilialiExtra { get; set; }
}
