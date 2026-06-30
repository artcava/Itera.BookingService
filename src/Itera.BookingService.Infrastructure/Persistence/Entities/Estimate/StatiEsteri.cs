namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class StatiEsteri
{
    public string CodiceNazione { get; set; } = string.Empty;
    public short ContinenteID { get; set; }
    public bool UnioneEuropea { get; set; }
    public string DescrizioneItaliana { get; set; } = string.Empty;
    public string DescrizioneInternazionale { get; set; } = string.Empty;
    public string? CodiceIso { get; set; }
    public string? CodiceIso2 { get; set; }
}
