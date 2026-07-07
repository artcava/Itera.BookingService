namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [dbo].[AccessorioFiliale].
/// </summary>
public sealed class AccessorioFiliale
{
    public int AccessorioFilialeID { get; set; }
    public short AccessorioTipologiaID { get; set; }
    public int FilialeID { get; set; }
    public string? Note { get; set; }

    public AccessorioTipologia AccessorioTipologia { get; set; } = null!;
    public Filiale Filiale { get; set; } = null!;
}