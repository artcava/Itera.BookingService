using Itera.BookingService.Contracts.Legacy;

public sealed class WsGetDefaultValuesRequest : ILegacyTokenCarrier
{
    public string? Token { get; set; }
    public short BrandID { get; set; }
    public int? BranchID { get; set; }
    public string? DebugDateToday { get; set; }  // opzionale, solo debug
    public byte LanguageID { get; set; }
}