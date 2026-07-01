namespace Itera.BookingService.Contracts.Options;

public class LegacyInfrastructureOptions
{
    public const string SectionName = "LegacyInfrastructure";

    public bool EnableDetailedErrors { get; set; }
    public int CommandTimeoutSeconds { get; set; } = 30;
}
