namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class WsTokenPreventivo
{
    public int WsTokenPreventivoID { get; set; }
    public int WsUserID { get; set; }
    public Guid Token { get; set; }
    public DateTime DataCreazione { get; set; }
    public DateTime DataUltimaModifica { get; set; }
    public DateTime DataFromPreventivo { get; set; }
    public DateTime DataToPreventivo { get; set; }
    public int FilialeID { get; set; }
    public int FilialeIDDestinazione { get; set; }
    public bool AssicurazioneExtra { get; set; }
    public string CodiceCategoria { get; set; } = string.Empty;
    public string? CodiceCoupon { get; set; }
    public string ObjectDynParam { get; set; } = string.Empty;
    public int? ListinoID { get; set; }
    public int? ListinoScontisticaID { get; set; }
    public string? SourceCampagnaMarketing { get; set; }
    public string? ObjectEstimate { get; set; }
    public bool? PrePagamento { get; set; }
    public int? PrecedenteWsTokenPreventivoID { get; set; }
    public int? Giorni { get; set; }
    public string? CodiceDurata { get; set; }
    public string? VoucherCliente { get; set; }
    public int? AccordoCommercialeID { get; set; }
    public bool? YoungDriver { get; set; }
    public string? SegmentAvailability { get; set; }

    public WsUser WsUser { get; set; } = null!;
    public Listino? Listino { get; set; }
    public WsTokenPreventivo? Precedente { get; set; }
    public ICollection<WsTokenPreventivo> Successivi { get; set; } = [];
}
