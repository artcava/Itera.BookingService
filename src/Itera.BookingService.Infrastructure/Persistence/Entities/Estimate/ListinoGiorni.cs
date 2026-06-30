namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class ListinoGiorni
{
    public int ListinoGiorniID { get; set; }
    public int ListinoID { get; set; }
    public string Codice { get; set; } = string.Empty;
    public string CodiceCategoria { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public int FasciaStart { get; set; }
    public int FasciaEnd { get; set; }
    public short Ordinamento { get; set; }
    public bool? IsVisible { get; set; }

    public Listino Listino { get; set; } = null!;
    public ICollection<ListinoKm> ListinoKm { get; set; } = [];
    public ICollection<ListinoValori> ListinoValori { get; set; } = [];
}
