namespace Itera.BookingService.Application.Security.Dtos;

/// <summary>
/// Struttura Data della WsResponse legacy: { Token: "guid-string" | null }
/// </summary>
public sealed record WsAuth(string? Token);

public sealed record GetTokenResponse(WsAuth Data);