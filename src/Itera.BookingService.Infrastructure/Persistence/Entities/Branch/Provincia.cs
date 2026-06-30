namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class Provincia
{
    public string SiglaAutomobilistica { get; set; } = string.Empty;
    public string? Denominazione { get; set; }
    public int? CodiceRegione { get; set; }
}
