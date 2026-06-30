namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class TipologiaFranchigia
{
    public string TipologiaFranchigiaID { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public byte? TipologiaVoceFatturaID { get; set; }
    public short? Priorita { get; set; }
    public string? Note { get; set; }
    public short? TipologiaFranchigiaCategoriaID { get; set; }

    public ICollection<ListinoFranchigia> ListinoFranchigie { get; set; } = [];
}
