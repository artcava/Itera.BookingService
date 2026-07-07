namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [pricing].[TariffaRdv].
/// </summary>
public sealed class TariffaRdv
{
    public int TariffaRdvID { get; set; }
    public int TariffarioID { get; set; }
    public short AccessorioTipologiaID { get; set; }
    public DateTime DataStart { get; set; }
    public DateTime DataEnd { get; set; }
    public short BreakEven { get; set; }
    public short MinGiorniApplicabilita { get; set; }
    public short MaxGiorniApplicabilita { get; set; }
    public decimal? Percentuale { get; set; }
    public decimal? ImportoFisso { get; set; }
    public string TipoImporto { get; set; } = null!;
    public decimal? ImportoGiornoExtra { get; set; }
    public decimal? ImportoMinAddebitabile { get; set; }
    public decimal? ImportoMaxAddebitabile { get; set; }
    public short? MaxGiorniAddebitabili { get; set; }
    public decimal? Tolleranza { get; set; }
    public string? StatoInclusione { get; set; }
    public string? Incasso { get; set; }
    public byte StatoID { get; set; }
    public DateTime DataInserimento { get; set; }
    public DateTime? DataUltimaModifica { get; set; }

    public Tariffario Tariffario { get; set; } = null!;
    public AccessorioTipologia AccessorioTipologia { get; set; } = null!;
}