namespace Itera.BookingService.Application.Security.Dtos;

public sealed record WsUserListinoDto(
    int IdListino,
    string CodiceListino,
    string DescrizioneListino);