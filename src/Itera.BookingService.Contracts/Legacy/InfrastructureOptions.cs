namespace Itera.BookingService.Contracts.Legacy;

public class LegacyInfrastructureOptions
{
    public const string SectionName = "LegacyInfrastructure";

    public bool EnableDetailedErrors { get; set; }
    public int CommandTimeoutSeconds { get; set; } = 30;
}
