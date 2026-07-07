namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [dbo].[CategoriaSegmento].
/// </summary>
public sealed class CategoriaSegmento
{
    public string CodiceCategoria { get; set; } = null!;
    public string Descrizione { get; set; } = null!;
    public string VoceContabile { get; set; } = null!;
    public string DescrizioneVoceContabile { get; set; } = null!;
    public string NomeFileImmagine { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public string? DescrizioneSAP { get; set; }

    public ICollection<TipologiaVoceFattura> TipologieVoceFattura { get; set; } = [];
}