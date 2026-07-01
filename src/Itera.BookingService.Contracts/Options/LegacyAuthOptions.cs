namespace Itera.BookingService.Contracts.Options;

public class LegacyAuthOptions
{
    public const string SectionName = "LegacyAuth";

    public string HeaderName { get; set; } = "X-Api-Token";

    public int TokenValidPeriodHours { get; set; } = 24;

    public bool EnablePayloadTokenFallback { get; set; } = true;
}
