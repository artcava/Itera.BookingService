using Itera.BookingService.Contracts.Abstractions;

namespace Itera.BookingService.Contracts.Estimate;

public sealed class GetDefaultValuesRequest : ITokenCarrier
{
    public string? Token { get; set; }
    public short BrandID { get; set; }
    public int? BranchID { get; set; }
    public string? DebugDateToday { get; set; }  // opzionale, solo debug
    public byte LanguageID { get; set; }
}
