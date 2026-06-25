namespace Itera.BookingService.Application.Branch;

internal static class LegacyRequestCultureDateResolver
{
    public static byte ResolveLinguaId(string? language)
    {
        return language?.Trim().ToLowerInvariant() switch
        {
            "eng" => 2,
            "ita" => 1,
            _ => 1
        };
    }

    public static DateTime ResolveDateStartLegacy(string? dateStart, byte linguaId)
    {
        if (string.IsNullOrWhiteSpace(dateStart))
        {
            return DateTime.Today;
        }

        var culture = linguaId == 2
            ? System.Globalization.CultureInfo.GetCultureInfo("en-US")
            : System.Globalization.CultureInfo.GetCultureInfo("it-IT");

        if (DateTime.TryParse(dateStart, culture, System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate.Date;
        }

        // Legacy parity: invalid DateStart falls back to today. This should be revisited once clients are aligned.
        return DateTime.Today;
    }
}
