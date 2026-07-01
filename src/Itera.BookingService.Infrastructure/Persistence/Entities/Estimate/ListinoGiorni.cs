namespace Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;

/// <summary>
/// Tabella [dbo].[ListinoGiorni].
/// Ogni riga rappresenta un periodo (fascia oraria) per una categoria veicolo.
/// </summary>
public sealed class ListinoGiorni
{
    public int    ListinoGiorniId  { get; set; }
    public int    ListinoId        { get; set; }
    public string Codice           { get; set; } = default!;
    public string CodiceCategoria  { get; set; } = default!;
    public string Descrizione      { get; set; } = default!;
    public int    FasciaStart      { get; set; }
    public int    FasciaEnd        { get; set; }
    public short  Ordinamento      { get; set; }
    public bool?  IsVisible        { get; set; }

    public Listino Listino { get; set; } = default!;
    public ICollection<ListinoKm> ListinoKm { get; set; } = [];
}
