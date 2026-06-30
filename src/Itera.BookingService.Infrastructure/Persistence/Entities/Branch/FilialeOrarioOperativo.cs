namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class FilialeOrarioOperativo
{
    public int FilialeOrarioOperativoID { get; set; }
    public int FilialeID { get; set; }
    public short StatoID { get; set; }
    public short GiornoSettimana { get; set; }
    public short Ordinamento { get; set; }
    public byte OraInizio { get; set; }
    public byte MinutiInizio { get; set; }
    public byte OraFine { get; set; }
    public byte MinutiFine { get; set; }
}
