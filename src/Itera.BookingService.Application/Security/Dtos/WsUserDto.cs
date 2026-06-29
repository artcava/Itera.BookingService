namespace Itera.BookingService.Application.Security.Dtos;

public sealed record WsUserDto(
    string UserId,
    string Username,
    string Nome,
    string Cognome,
    string Email,
    string CodiceFiliale,
    bool Attivo,
    IReadOnlyList<WsUserGruppoDto> Gruppi,
    IReadOnlyList<WsUserListinoDto> Listini);