namespace Itera.BookingService.Application.Security.Dtos;

public sealed record UserListinoDto(
    int IdListino,
    string CodiceListino,
    string DescrizioneListino);
