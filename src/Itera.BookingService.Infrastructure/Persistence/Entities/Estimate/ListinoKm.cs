namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class ListinoKm
{
    public int ListinoKmID { get; set; }
    public int ListinoGiorniID { get; set; }
    public int Km { get; set; }
    public short Ordinamento { get; set; }
    public bool? IsVisible { get; set; }

    public ListinoGiorni ListinoGiorni { get; set; } = null!;
}
