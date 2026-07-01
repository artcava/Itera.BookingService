namespace Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;

/// <summary>
/// Tabella [dbo].[Listino] — solo colonne necessarie alla query GetKms.
/// </summary>
public sealed class Listino
{
    public int       ListinoId        { get; set; }
    public DateTime? InizioValidita   { get; set; }
    public DateTime? FineValidita     { get; set; }
    public bool      SempreAttivo     { get; set; }

    public ICollection<ListinoGiorni> ListinoGiorni { get; set; } = [];
    public ICollection<ListinoFiliale> ListinoFiliali { get; set; } = [];
}
