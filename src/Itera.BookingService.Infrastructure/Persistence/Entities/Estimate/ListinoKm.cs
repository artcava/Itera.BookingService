namespace Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;

/// <summary>
/// Tabella [dbo].[ListinoKm].
/// Ogni riga rappresenta un'opzione km disponibile per un dato ListinoGiorni.
/// </summary>
public sealed class ListinoKm
{
    public int   ListinoKmId      { get; set; }
    public int   ListinoGiorniId  { get; set; }

    /// <summary>Valore km (es. 100, 200, 0 = illimitati).</summary>
    public int   Km               { get; set; }
    public short Ordinamento      { get; set; }
    public bool? IsVisible        { get; set; }

    public ListinoGiorni ListinoGiorni { get; set; } = default!;
}
