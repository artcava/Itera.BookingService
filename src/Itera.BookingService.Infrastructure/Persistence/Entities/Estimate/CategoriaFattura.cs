namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [dbo].[CategoriaFattura].
/// </summary>
public sealed class CategoriaFattura
{
    public byte CategoriaFatturaID { get; set; }
    public string Descrizione { get; set; } = null!;
    public short? IvaIDPredefinita { get; set; }

    public Iva? IvaPredefinita { get; set; }

    public ICollection<TipologiaVoceFattura> TipologieVoceFattura { get; set; } = [];
}