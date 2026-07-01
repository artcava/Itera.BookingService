namespace Itera.BookingService.Contracts.Legacy.Estimate;

/// <summary>
/// Singola opzione km disponibile per un listino/durata.
/// </summary>
public sealed class KmOpzione
{
    /// <summary>Identificativo dell'opzione km (es. "100", "UNLI").</summary>
    public string? KmId { get; set; }

    /// <summary>
    /// Descrizione dell'opzione.
    /// ATTENZIONE: il nome JSON è "Decription" (typo legacy) — NON correggere
    /// senza allineamento con i client.
    /// </summary>
    public string? Descrizione { get; set; }

    /// <summary>Indica se questa opzione è quella di default/preselezionata.</summary>
    public bool Selected { get; set; }
}
