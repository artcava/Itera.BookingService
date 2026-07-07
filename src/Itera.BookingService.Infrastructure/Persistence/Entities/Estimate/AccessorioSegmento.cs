namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [dbo].[AccessorioSegmento].
/// </summary>
public sealed class AccessorioSegmento
{
    public int AccessorioSegmentoID { get; set; }
    public short AccessorioTipologiaID { get; set; }
    public string CodiceSegmento { get; set; } = null!;

    public AccessorioTipologia AccessorioTipologia { get; set; } = null!;
    public SegmentoModello SegmentoModello { get; set; } = null!;
}