namespace Itera.BookingService.Application.Security.Dtos;

public sealed record GetTokenRequest(
    string Username,
    string Password);