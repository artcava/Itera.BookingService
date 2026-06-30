namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class SegmentoModello
{
    public string CodiceSegmento { get; set; } = string.Empty;
    public string CodiceCategoria { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public byte Ordinamento { get; set; }
    public int? ModelloMezzoIDPdfOfferta { get; set; }
    public byte? Stato { get; set; }
    public int? ModelloMezzoIDErs { get; set; }
    public string? FleetID { get; set; }
    public int? SegmentoModelloClasseID { get; set; }
    public short? IndexPricing { get; set; }
    public int? ListinoID { get; set; }
    public decimal? ImportoVAL { get; set; }
    public DateTime? DataInserimento { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
}
