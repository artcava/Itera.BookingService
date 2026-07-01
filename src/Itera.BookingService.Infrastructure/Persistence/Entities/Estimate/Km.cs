namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class Km
{
    public int KmID { get; set; }
    public string DurataID { get; set; } = string.Empty;
    public string CodiceCategoria { get; set; } = string.Empty;
    public int Giorni { get; set; }
    public int? Inizio { get; set; }
    public int? Fine { get; set; }
    public string Descrizione { get; set; } = string.Empty;
    public int Ordinamento { get; set; }
}
