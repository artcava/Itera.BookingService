namespace Itera.BookingService.Infrastructure.Persistence.Entities;

/// <summary>
/// Tabella [dbo].[AccessorioCategoria].
/// </summary>
public sealed class AccessorioCategoria
{
    public short AccessorioCategoriaID { get; set; }
    public string Descrizione { get; set; } = null!;
    public string Codice { get; set; } = null!;
    public short? BrandID { get; set; }
    public byte StatoID { get; set; }

    public Brand? Brand { get; set; }

    public ICollection<AccessorioTipologia> AccessorioTipologie { get; set; } = [];
}