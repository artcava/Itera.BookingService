namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class SegmentoModelloClasse
{
    public int SegmentoModelloClasseID { get; set; }
    public string? Descrizione { get; set; }
    public DateTime? DataInserimento { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
    public int? LeadDays { get; set; }
}
