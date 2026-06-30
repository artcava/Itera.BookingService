namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class FilialeRiposoSettimanale
{
    public short FilialeRiposoSettimanaleID { get; set; }
    public int FilialeID { get; set; }
    public byte GiornoSettimana { get; set; }
}
