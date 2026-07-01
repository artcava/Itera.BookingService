namespace Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;

/// <summary>
/// Tabella di associazione [dbo].[ListinoFiliale].
/// PK composita (FilialeID, ListinoID).
/// </summary>
public sealed class ListinoFiliale
{
    public int FilialeId  { get; set; }
    public int ListinoId  { get; set; }

    public ListinoGiorni Listino { get; set; } = default!;
}
