namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [pricing].[Tariffario].
/// </summary>
public sealed class Tariffario
{
    public int TariffarioID { get; set; }
    public string? Descrizione { get; set; }
    public short? BrandID { get; set; }
    public string? Codice { get; set; }

    public Brand? Brand { get; set; }

    public ICollection<TariffaRdv> TariffeRdv { get; set; } = [];
}