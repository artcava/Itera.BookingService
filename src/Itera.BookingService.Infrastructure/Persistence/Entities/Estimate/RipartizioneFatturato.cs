namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [dbo].[RipartizioneFatturato].
/// </summary>
public sealed class RipartizioneFatturato
{
    public string RipartizioneFatturatoID { get; set; } = null!;
    public string? Descrizione { get; set; }

    public ICollection<TipologiaVoceFattura> TipologieVoceFattura { get; set; } = [];
}