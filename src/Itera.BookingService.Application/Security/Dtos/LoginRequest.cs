namespace Itera.BookingService.Application.Security.Dtos;

public sealed record LoginRequest(
    string Username,
    string Password,
    string CodiceFiliale);