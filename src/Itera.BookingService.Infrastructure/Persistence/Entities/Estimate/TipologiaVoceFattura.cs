namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [dbo].[TipologiaVoceFattura].
/// </summary>
public sealed class TipologiaVoceFattura
{
    public byte TipologiaVoceFatturaID { get; set; }
    public string Descrizione { get; set; } = null!;
    public string? DescrizioneFatturazione { get; set; }
    public string? CodArticoloFatturazione { get; set; }
    public bool IsNotaCredito { get; set; }
    public byte? CategoriaFatturaID { get; set; }
    public string? CodiceCategoriaSegmento { get; set; }
    public string? RipartizioneFatturatoID { get; set; }
    public bool Attiva { get; set; }
    public string? CodArticoloFatturazioneSostitutivo { get; set; }
    public DateTime? DataInserimento { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
    public bool? FatturazioneSuProprietaFisicaMezzo { get; set; }
    public short? IvaID { get; set; }

    public CategoriaFattura? CategoriaFattura { get; set; }
    public CategoriaSegmento? CategoriaSegmento { get; set; }
    public RipartizioneFatturato? RipartizioneFatturato { get; set; }
    public Iva? Iva { get; set; }

    public ICollection<AccessorioTipologia> AccessorioTipologie { get; set; } = [];
    public ICollection<AccessorioTipologia> AccessorioTipologiePenale { get; set; } = [];
}