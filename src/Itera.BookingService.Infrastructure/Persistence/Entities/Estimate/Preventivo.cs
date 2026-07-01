using Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;

namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class Preventivo
{
    public int PreventivoID { get; set; }
    public DateTime DataDocumento { get; set; }
    public string CodiceCategoria { get; set; } = string.Empty;
    public string CodiceSegmento { get; set; } = string.Empty;
    public string CodiceDurata { get; set; } = string.Empty;
    public int KmID { get; set; }
    public short Giorni { get; set; }
    public DateTime? DataInizio { get; set; }
    public DateTime? DataFine { get; set; }
    public int ListinoID { get; set; }
    public int? ListinoScontisticaID { get; set; }
    public bool AssicurazioneExtra { get; set; }
    public int FilialeID { get; set; }
    public int OperatoreID { get; set; }
    public decimal Importo { get; set; }
    public short IvaID { get; set; }
    public int ContatoreCliente { get; set; }
    public int? FilialeDestinazioneID { get; set; }
    public bool? VAL { get; set; }
    public decimal? VALImporto { get; set; }

    public Listino Listino { get; set; } = null!;
}
