namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class AccordoCommercialeListino
{
    public int AccordoCommercialeListinoID { get; set; }
    public int AccordoCommercialeID { get; set; }
    public int ListinoID { get; set; }
    public DateTime PeriodoValiditaDa { get; set; }
    public DateTime PeriodoValiditaA { get; set; }
    public bool ProdottoEsclusivo { get; set; }
    public DateTime? DataInserimento { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
    public int TariffarioID { get; set; }
    public decimal? ScontoPenaleRisarcitoriaFurto { get; set; }
    public decimal? ScontoPenaleRisarcitoriaDanni { get; set; }
    public decimal? ScontoPenaleRisarcitoriaRidottaFurto { get; set; }
    public decimal? ScontoPenaleRisarcitoriaRidottaDanni { get; set; }

    public Listino Listino { get; set; } = null!;
}
