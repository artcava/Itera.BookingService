namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class PrenotazioneAccessorio
{
    public int PrenotazioneAccessorioID { get; set; }
    public int PrenotazioneID { get; set; }
    public int AccessorioID { get; set; }

    public Accessorio Accessorio { get; set; } = null!;
}
