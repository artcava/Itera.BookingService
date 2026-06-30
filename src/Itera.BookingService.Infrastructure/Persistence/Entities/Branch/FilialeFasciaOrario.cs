namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class FilialeFasciaOrario
{
    public short FilialeFasciaOrarioID { get; set; }
    public short OraInizio { get; set; }
    public short OraFine { get; set; }
    public short TipologiaFasciaOrarioID { get; set; }
    public byte StatoID { get; set; }
    public short ModificatoreGiorno { get; set; }
    public int FilialeID { get; set; }
    public short Ordinamento { get; set; }
    public short MinutiInizio { get; set; }
    public short MinutiFine { get; set; }
    public bool DefaultSelected { get; set; }
    public bool DefaultSelectedWeb { get; set; }
    public string? PeriodoDelGiornoID { get; set; }
}
