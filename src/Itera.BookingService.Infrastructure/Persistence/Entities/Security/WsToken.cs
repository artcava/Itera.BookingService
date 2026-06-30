namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class WsToken
{
    public int WsTokenID { get; set; }
    public int WsUserID { get; set; }
    public Guid Token { get; set; }
    public short BrandID { get; set; }
    public DateTime? DataCreazione { get; set; }
    public DateTime? DataUltimaModifica { get; set; }

    public WsUser WsUser { get; set; } = null!;
}
