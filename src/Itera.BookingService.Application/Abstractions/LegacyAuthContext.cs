namespace Itera.BookingService.Application.Abstractions;

public sealed class LegacyAuthContext
{
    public const string ItemKey = "LegacyAuthContext";

    public required string Token { get; init; }
    public required int WsUserId { get; init; }
    public required short BrandId { get; init; }
}
