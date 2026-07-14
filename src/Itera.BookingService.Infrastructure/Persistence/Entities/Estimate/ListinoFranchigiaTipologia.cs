namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class ListinoFranchigiaTipologia
{
    public int ListinoID { get; set; }

    public string TipologiaFranchigiaID { get; set; } = string.Empty;

    public short StatoID { get; set; }

    public string? StatoInclusione { get; set; }
}