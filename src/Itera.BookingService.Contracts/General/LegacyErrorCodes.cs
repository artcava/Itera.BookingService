namespace Itera.BookingService.Contracts.General;

public static class LegacyErrorCodes
{
    public const int Success = 0;
    public const int InvalidToken = -100;
    public const int ExpiredToken = -101;
    public const int InvalidFilialeId = -201;
    public const int FilialeNotFound = -204;
}
