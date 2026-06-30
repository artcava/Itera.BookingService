namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class FilialeOrarioOperativoVariazione
{
    public int FilialeOrarioOperativoVariazioneID { get; set; }
    public int FilialeID { get; set; }
    public DateTime GiornoDa { get; set; }
    public DateTime GiornoA { get; set; }
    public string? GiorniSettimana { get; set; }
    public short Ordinamento { get; set; }
    public byte OraInizio { get; set; }
    public byte MinutiInizio { get; set; }
    public byte OraFine { get; set; }
    public byte MinutiFine { get; set; }
    public string? TipologiaOrarioOperativo { get; set; }
    public int Priorita { get; set; }
    public short StatoID { get; set; }
}
