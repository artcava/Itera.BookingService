namespace Itera.BookingService.Contracts.Estimate;

public record BaseRequest
{
    public bool? ForceControlRentalSoftware360 { get; init; }
    public int? NumberPage { get; init; }
    public int? ItemForPage { get; init; }
    public string Language { get; init; } = string.Empty;
    public int? ClientID { get; init; }
    public string AccountID { get; init; } = string.Empty;
    public string Protocol { get; init; } = string.Empty;
    public int? UserID { get; init; }
    public int WsUserID { get; init; }
    public short BrandID { get; init; }
}