namespace Itera.BookingService.Contracts.Legacy.Branch;

public class GetAllBranchesRequest : ILegacyTokenCarrier
{
    public string? Token { get; set; }
    public string? Language { get; set; }
    public string? DateStart { get; set; }
    public bool GetExtraData { get; set; }
    public bool? GetFilialiExtra { get; set; }
}
