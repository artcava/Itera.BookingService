namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class Listino
{
    public int ListinoID { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public DateTime DataCreazione { get; set; }
    public DateTime? InizioValidita { get; set; }
    public DateTime? FineValidita { get; set; }
    public bool IsGrandiGruppi { get; set; }
    public byte Ordinamento { get; set; }
    public bool SempreAttivo { get; set; }
    public byte Stato { get; set; }
    public int? ListinoRaggruppamentoID { get; set; }
    public int? OperatoreID { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
    public int? ListinoIDPadre { get; set; }
    public short? Versione { get; set; }
    public string? VersioneNome { get; set; }
    public DateTime? DataCreazioneOriginale { get; set; }
    public byte? StatoVisibilita { get; set; }
    public Guid GuidListino { get; set; }
    public DateTime? DataInserimento { get; set; }
    public int TariffarioID { get; set; }
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
    public TimeOnly? OrarioInizioWeekend { get; set; }
    public TimeOnly? OrarioFineWeekend { get; set; }
    public bool FullCredit { get; set; }

    public Listino? ListinoPadre { get; set; }
    public ICollection<Listino> ListiniFigli { get; set; } = [];
    public ICollection<ListinoGiorni> ListinoGiorni { get; set; } = [];
    public ICollection<ListinoValori> ListinoValori { get; set; } = [];
    public ICollection<ListinoFranchigia> ListinoFranchigie { get; set; } = [];
    public ICollection<AccordoCommercialeListino> AccordiCommercialiListino { get; set; } = [];
    public ICollection<RegolaDiVenditaListino> RegoleDiVendita { get; set; } = [];
    public ICollection<WsTokenPreventivo> WsTokenPreventivi { get; set; } = [];
    public ICollection<Preventivo> Preventivi { get; set; } = [];
}
