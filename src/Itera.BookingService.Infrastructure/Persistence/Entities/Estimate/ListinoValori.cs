namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class ListinoValori
{
    public int ListinoValoriID { get; set; }
    public int? ListinoID { get; set; }
    public int? ListinoGiorniID { get; set; }
    public int? ListinoKmID { get; set; }
    public string? CodiceCategoria { get; set; }
    public string CodiceDurata { get; set; } = string.Empty;
    public int? Giorni { get; set; }
    public string? CodiceSegmento { get; set; }
    public decimal? ValoreBase { get; set; }
    public decimal? ValoreExtra { get; set; }

    public Listino? Listino { get; set; }
    public ListinoGiorni? ListinoGiorni { get; set; }
}
