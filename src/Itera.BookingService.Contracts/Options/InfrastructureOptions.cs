namespace Itera.BookingService.Contracts.Options;

public class InfrastructureOptions
{
    public const string SectionName = "Infrastructure";

    public bool EnableDetailedErrors { get; set; }
    public int CommandTimeoutSeconds { get; set; } = 30;
}
