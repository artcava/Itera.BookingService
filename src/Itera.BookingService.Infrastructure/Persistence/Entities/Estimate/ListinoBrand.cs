using Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;

namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class ListinoBrand
{
    public int ListinoBrandID { get; set; }
    public short BrandID { get; set; }
    public int ListinoID { get; set; }
    public DateTime? DataInserimento { get; set; }
    public DateTime? DataUltimaModifica { get; set; }

    public Listino Listino { get; set; } = null!;
}
