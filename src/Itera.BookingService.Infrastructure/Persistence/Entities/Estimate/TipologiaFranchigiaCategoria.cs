namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class TipologiaFranchigiaCategoria
{
    public short TipologiaFranchigiaCategoriaID { get; set; }
    public string Descrizione { get; set; } = string.Empty;
    public string Codice { get; set; } = string.Empty;
    public byte StatoID { get; set; }
}