namespace Itera.BookingService.Application.Security.Dtos;

public sealed record ResetKeyCacheRequest(
    string Token,
    string? KeySqlCache);