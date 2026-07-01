using Itera.BookingService.Contracts.Abstractions;

namespace Itera.BookingService.Contracts.Branch;

public class GetAllBranchesRequest : ITokenCarrier
{
    public string? Token { get; set; }
    public string? Language { get; set; }
    public string? DateStart { get; set; }
    public bool GetExtraData { get; set; }
    public bool? GetFilialiExtra { get; set; }
}
