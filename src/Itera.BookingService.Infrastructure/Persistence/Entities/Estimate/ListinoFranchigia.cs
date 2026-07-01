using Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;

namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class ListinoFranchigia
{
    public int ListinoFranchigiaID { get; set; }
    public string DurataID { get; set; } = string.Empty;
    public string CodiceSegmento { get; set; } = string.Empty;
    public decimal PenaleRisarcitoriaRCAuto { get; set; }
    public decimal PenaleRisarcitoriaDanni { get; set; }
    public decimal PenaleRisarcitoriaIncendioFurto { get; set; }
    public decimal CostoCoperturaExtra { get; set; }
    public decimal CostoCoperturaExtraSoglia { get; set; }
    public decimal PenaleRisarcitoriaDanniRidotta { get; set; }
    public decimal PenaleRisarcitoriaIncendioFurtoRidotta { get; set; }
    public DateTime ValidaDal { get; set; }
    public DateTime? ValidaAl { get; set; }
    public int ListinoID { get; set; }
    public string TipologiaFranchigiaID { get; set; } = string.Empty;
    public DateTime? DataUltimaModifica { get; set; }
    public DateTime? DataInserimento { get; set; }
    public string? SubCodice { get; set; }
    public short? BreakEven { get; set; }
    public short? MinGiorniApplicabilita { get; set; }
    public short? MaxGiorniApplicabilita { get; set; }
    public string? TipoImporto { get; set; }
    public decimal? ImportoGiornoExtra { get; set; }

    public Listino Listino { get; set; } = null!;
    public TipologiaFranchigia TipologiaFranchigia { get; set; } = null!;
}
