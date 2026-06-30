namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class WsUser
{
    public int WsUserID { get; set; }
    public string Username { get; set; } = string.Empty;
    public string SecretWord { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public bool? AccountTest { get; set; }
    public bool? AccettaSegmentoNonVendibile { get; set; }
    public bool? DisponibilitaMacroArea { get; set; }
    public int? ListinoScontisticaID { get; set; }
    public short? BrandID { get; set; }
    public bool? DisponibilitaRaggruppamento { get; set; }

    public ICollection<WsToken> WsTokens { get; set; } = [];
}
