namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Lookup generico degli stati. PK composta: (Codice, Tipologia).
/// </summary>
public class StatoElemento
{
    public short Codice { get; set; }
    public string? Descrizione { get; set; }
    public string Tipologia { get; set; } = string.Empty;
}