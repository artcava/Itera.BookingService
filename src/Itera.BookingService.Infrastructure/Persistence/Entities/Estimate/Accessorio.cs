namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class Accessorio
{
    public int AccessorioID { get; set; }
    public string ChiaveAccessorio { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public short? Ordinamento { get; set; }

    public ICollection<PrenotazioneAccessorio> PrenotazioniAccessorio { get; set; } = [];
}
