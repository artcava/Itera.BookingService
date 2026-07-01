namespace Itera.BookingService.Application.Security.Dtos;

/// <summary>
/// Struttura Data della risposta legacy GetToken: { Token: "guid-string" | null }
/// </summary>
public sealed record AuthTokenData(string? Token);

public sealed record GetTokenResponse(AuthTokenData Data);
