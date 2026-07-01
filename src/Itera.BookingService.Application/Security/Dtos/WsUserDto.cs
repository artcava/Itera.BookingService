namespace Itera.BookingService.Application.Security.Dtos;

public sealed record UserDto(
    string UserId,
    string Username,
    string Nome,
    string Cognome,
    string Email,
    string CodiceFiliale,
    bool Attivo,
    IReadOnlyList<UserGruppoDto> Gruppi,
    IReadOnlyList<UserListinoDto> Listini);
