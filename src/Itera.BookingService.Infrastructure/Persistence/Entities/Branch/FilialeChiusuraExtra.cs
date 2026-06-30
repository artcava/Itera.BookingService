namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class FilialeChiusuraExtra
{
    public int FilialeChiusuraExtraID { get; set; }
    public int FilialeID { get; set; }
    public int Giorno { get; set; }
    public int Mese { get; set; }
    public int? Anno { get; set; }
}
