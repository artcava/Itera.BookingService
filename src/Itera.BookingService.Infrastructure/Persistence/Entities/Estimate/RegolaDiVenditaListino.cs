namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class RegolaDiVenditaListino
{
    public int RegolaDiVenditaListinoID { get; set; }
    public int RegolaDiVenditaID { get; set; }
    public int? ListinoID { get; set; }
    public int? ListinoRaggruppamentoID { get; set; }

    public Listino? Listino { get; set; }
}
