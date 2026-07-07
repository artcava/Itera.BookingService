namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class AccessorioTipologia
{
    public short AccessorioTipologiaID { get; set; }
    public short AccessorioCategoriaID { get; set; }
    public string Codice { get; set; } = null!;
    public short? BrandID { get; set; }
    public bool Obbligatorio { get; set; }
    public short? IvaID { get; set; }
    public bool OneriAptRw { get; set; }
    public bool ImportoForzato { get; set; }
    public bool InseribileFiliale { get; set; }
    public bool ModificabileFiliale { get; set; }
    public bool PrepagamentoWeb { get; set; }
    public bool PrepagamentoContactCenter { get; set; }
    public bool VendibilitaWeb { get; set; }
    public bool VendibilitaContactCenter { get; set; }
    public bool VendibilitaFiliale { get; set; }
    public byte TipologiaVoceFatturaID { get; set; }
    public decimal PercentualeShort { get; set; }
    public decimal PercentualeOpen { get; set; }
    public decimal PercentualeAccessorio { get; set; }
    public byte StatoID { get; set; }
    public DateTime? DataInizioValidita { get; set; }
    public DateTime? DataFineValidita { get; set; }
    public bool SplitInFattura { get; set; }
    public decimal PercentualeSplit { get; set; }
    public decimal? ImportoPenale { get; set; }
    public byte? TipologiaVoceFatturaIDPenale { get; set; }
    public bool Preselezionato { get; set; }
    public string? MomentoVendibilita { get; set; }
    public short? AccessorioTipologiaIDPenale { get; set; }
    public bool Vendibile { get; set; }

    public virtual AccessorioCategoria AccessorioCategoria { get; set; } = null!;
    public virtual Brand? Brand { get; set; }
    public virtual Iva? Iva { get; set; }
    public virtual TipologiaVoceFattura TipologiaVoceFattura { get; set; } = null!;
    public virtual TipologiaVoceFattura? TipologiaVoceFatturaPenale { get; set; }
    public virtual AccessorioTipologia? AccessorioTipologiaPenale { get; set; }
    public ICollection<AccessorioFiliale> AccessorioFiliali { get; set; } = [];
    public ICollection<AccessorioSegmento> AccessorioSegmenti { get; set; } = [];
    public ICollection<TariffaRdv> TariffeRdv { get; set; } = [];
    public virtual ICollection<AccessorioTipologia> AccessoriTipologiaPenaleFigli { get; set; }
        = new List<AccessorioTipologia>();
}