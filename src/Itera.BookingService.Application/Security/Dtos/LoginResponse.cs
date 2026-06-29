namespace Itera.BookingService.Application.Security.Dtos;

public sealed record LoginResponse(
    string Token,
    string UserId,
    string Username,
    string NomeCompleto,
    string CodiceFiliale,
    IReadOnlyList<string> Gruppi);