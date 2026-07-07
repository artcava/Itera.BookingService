namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [dbo].[Listino].
/// </summary>
public sealed class Listino
{
    public int ListinoId { get; set; }
    public string Tipo { get; set; } = null!;
    public string Descrizione { get; set; } = null!;
    public DateTime DataCreazione { get; set; }
    public DateTime? InizioValidita { get; set; }
    public DateTime? FineValidita { get; set; }
    public bool IsGrandiGruppi { get; set; }
    public byte Ordinamento { get; set; }
    public bool SempreAttivo { get; set; }
    public byte Stato { get; set; }
    public int? ListinoRaggruppamentoId { get; set; }
    public int? OperatoreId { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
    public int? ListinoIdPadre { get; set; }
    public short? Versione { get; set; }
    public string? VersioneNome { get; set; }
    public DateTime? DataCreazioneOriginale { get; set; }
    public byte? StatoVisibilita { get; set; }
    public Guid GuidListino { get; set; }
    public DateTime? DataInserimento { get; set; }
    public int TariffarioId { get; set; }
    public bool EsclusioneWalkIn { get; set; }
    public short? TolleranzaOraria { get; set; }
    public bool ProdottoOrario { get; set; }
    public string? ProdottoOrarioTipoImporto { get; set; }
    public decimal? ProdottoOrarioPercentuale { get; set; }
    public decimal? ProdottoOrarioImporto { get; set; }
    public int? DurataMinimaNolo { get; set; }
    public int? DurataMassimaNolo { get; set; }
    public int? DurataMinimaAddebitabile { get; set; }
    public byte TipologiaCalcoloWeekend { get; set; }
    public byte? GiornoInizioWeekend { get; set; }
    public byte? GiornoFineWeekend { get; set; }
    public TimeSpan? OrarioInizioWeekend { get; set; }
    public TimeSpan? OrarioFineWeekend { get; set; }
    public bool FullCredit { get; set; }

    public Tariffario Tariffario { get; set; } = null!;
    public Listino? ListinoPadre { get; set; }
    public ICollection<Listino> ListiniFigli { get; set; } = [];

    public ICollection<ListinoGiorni> ListinoGiorni { get; set; } = [];
    public ICollection<ListinoFiliale> ListinoFiliali { get; set; } = [];
}