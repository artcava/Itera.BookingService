using Itera.BookingService.Contracts.Abstractions;

namespace Itera.BookingService.Contracts.Branch;

public class GetBranchInfoRequest : ILegacyTokenCarrier
{
    public string? Token { get; set; }
    public string? Language { get; set; }
    public string? DateStart { get; set; }
    public int BranchID { get; set; }
    public bool? GetFilialiExtra { get; set; }
}
