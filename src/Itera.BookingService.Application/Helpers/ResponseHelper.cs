using Itera.BookingService.Contracts.General;

namespace Itera.BookingService.Application.Helpers;

public static class ResponseHelper
{
    public static ApiResponse<T> LegacyError<T>(int errorCode, string message)
    {
        return new ApiResponse<T>
        {
            Esito = false,
            CodiceErrore = errorCode.ToString(),
            Messaggio = message,
            Data = default
        };
    }

    public static ApiResponse<T> LegacyError<T>(string errorCode, string message)
    {
        return new ApiResponse<T>
        {
            Esito = false,
            CodiceErrore = errorCode,
            Messaggio = message,
            Data = default
        };
    }
}
