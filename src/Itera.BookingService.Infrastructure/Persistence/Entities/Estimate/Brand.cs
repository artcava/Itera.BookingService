namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [dbo].[Brand].
/// </summary>
public sealed class Brand
{
    public short BrandID { get; set; }
    public string CodiceBrand { get; set; } = null!;
    public string? Descrizione { get; set; }
    public DateTime? DataInserimento { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
    public string? DescrizioneSAP { get; set; }
    public string? DescrizioneMit { get; set; }
    public string? LogoImmagine { get; set; }
    public string? EmailFrom { get; set; }
    public string? EmailFromAlias { get; set; }

    public ICollection<AccessorioCategoria> AccessorioCategorie { get; set; } = [];
    public ICollection<AccessorioTipologia> AccessorioTipologie { get; set; } = [];
}