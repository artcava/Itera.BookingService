namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class Mezzo
{
    public string CodiceMezzo { get; set; } = string.Empty;
    public int ModelloMezzoID { get; set; }
    public short TipoMezzoID { get; set; }
    public short StatoMezzoID { get; set; }
    public string? Targa { get; set; }
    public string? Telaio { get; set; }
    public string? CodiceAutoradio { get; set; }
    public string? KeyCode { get; set; }
    public bool FAP { get; set; }
    public int? KmEffettuati { get; set; }
    public string? ColoreInterno { get; set; }
    public string? ColoreEsterno { get; set; }
    public int ContrattoMezzoID { get; set; }
    public int AssicurazioneMezzoID { get; set; }
    public short? AllestimentoMezzoID { get; set; }
    public bool Pianalatura { get; set; }
    public bool AllestimentoOK { get; set; }
    public bool LoghiEsterni { get; set; }
    public int? FilialeID { get; set; }
    public byte? StatoCondizioneMezzoID { get; set; }
    public int ProgressivoAcquisto { get; set; }
    public string? CodiceMezzoFinale { get; set; }
    public DateTime? DataEvasione { get; set; }
    public byte? CarburanteMezzoID { get; set; }
    public DateTime? DataFineContratto { get; set; }
    public bool? IsEsportato { get; set; }
    public string? CodiceMezzoVisualizzato { get; set; }
    public string? Note { get; set; }
    public string? SubCodice { get; set; }
    public int ProprietaFisica { get; set; }
    public int ProprietaLogica { get; set; }
    public DateTime? DataImmatricolazione { get; set; }
    public bool? Hold { get; set; }
    public DateTime? DataFineCanoni { get; set; }
    public DateTime? DataModificaHold { get; set; }
    public int? FranchiseID { get; set; }
    public short BrandID { get; set; }
    public bool? Evadibile { get; set; }
    public bool Lucchetto { get; set; }
    public bool ValidoPerGdpr { get; set; }
    public int? SottoFacilityID { get; set; }
    public bool? GommeTermiche { get; set; }
}
