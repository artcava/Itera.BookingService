namespace Itera.BookingService.Contracts.Options;

public class AuthOptions
{
    public const string SectionName = "Auth";

    public string HeaderName { get; set; } = "X-Api-Token";

    public int TokenValidPeriodHours { get; set; } = 24;

    public bool EnablePayloadTokenFallback { get; set; } = true;
}
