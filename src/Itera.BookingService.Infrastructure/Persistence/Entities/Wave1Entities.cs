namespace Itera.BookingService.Infrastructure.Persistence;

public class Filiale
{
    public int FilialeID { get; set; }
    public int FranchiseID { get; set; }
    public string? Indirizzo { get; set; }
    public string? Cap { get; set; }
    public string? Citta { get; set; }
    public string? Telefono { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? SiglaAutomobilistica { get; set; }
    public double? CoordX { get; set; }
    public double? CoordY { get; set; }
    public double? SpnX { get; set; }
    public double? SpnY { get; set; }
    public short? Zoom { get; set; }
    public short? Stato { get; set; }
    public string? Parcheggio { get; set; }
    public string? RespCommerciale { get; set; }
    public string? RespAmministrazione { get; set; }
    public bool? KeyBox { get; set; }
    public bool? EsclusioneVal { get; set; }
    public string? FleetNonVendibile { get; set; }
    public int FilialeAreaID { get; set; }
    public int? FilialeClassificazioneID { get; set; }
}

public class FilialeTesto
{
    public int FilialeID { get; set; }
    public string? Descrizione { get; set; }
    public string? OrariUfficio { get; set; }
    public string? OrariConsegna { get; set; }
}

public class Franchise
{
    public int FranchiseID { get; set; }
    public string? Descrizione { get; set; }
    public string? CodiceAzienda { get; set; }
}

public class FilialeArea
{
    public int FilialeAreaID { get; set; }
    public int FilialeMacroAreaID { get; set; }
}

public class FilialeClassificazione
{
    public int FilialeClassificazioneID { get; set; }
    public string? Descrizione { get; set; }
}

public class Provincia
{
    public string SiglaAutomobilistica { get; set; } = string.Empty;
    public string? Denominazione { get; set; }
    public int? CodiceRegione { get; set; }
}

public class Regione
{
    public int CodiceRegione { get; set; }
    public string? Denominazione { get; set; }
}

public class FilialeBrand
{
    public int FilialeBrandID { get; set; }
    public int FilialeID { get; set; }
    public short BrandID { get; set; }
}

public class Testo
{
    public int TestoID { get; set; }
    public string? Chiave { get; set; }
    public byte LinguaID { get; set; }
    public string? Valore { get; set; }
}

public class FilialeChiusuraExtra
{
    public int FilialeChiusuraExtraID { get; set; }
    public int FilialeID { get; set; }
    public int Giorno { get; set; }
    public int Mese { get; set; }
    public int? Anno { get; set; }
}

public class FilialeOrarioOperativoVariazione
{
    public int FilialeOrarioOperativoVariazioneID { get; set; }
    public int FilialeID { get; set; }
    public DateTime GiornoDa { get; set; }
    public DateTime GiornoA { get; set; }
    public string? GiorniSettimana { get; set; }
    public short Ordinamento { get; set; }
    public byte OraInizio { get; set; }
    public byte MinutiInizio { get; set; }
    public byte OraFine { get; set; }
    public byte MinutiFine { get; set; }
    public string? TipologiaOrarioOperativo { get; set; }
    public int Priorita { get; set; }
    public short StatoID { get; set; }
}

public class FilialeOrarioOperativo
{
    public int FilialeOrarioOperativoID { get; set; }
    public int FilialeID { get; set; }
    public short StatoID { get; set; }
    public short GiornoSettimana { get; set; }
    public short Ordinamento { get; set; }
    public byte OraInizio { get; set; }
    public byte MinutiInizio { get; set; }
    public byte OraFine { get; set; }
    public byte MinutiFine { get; set; }
}

public class FilialeFasciaOrario
{
    public short FilialeFasciaOrarioID { get; set; }
    public short OraInizio { get; set; }
    public short OraFine { get; set; }
    public short TipologiaFasciaOrarioID { get; set; }
    public byte StatoID { get; set; }
    public short ModificatoreGiorno { get; set; }
    public int FilialeID { get; set; }
    public short Ordinamento { get; set; }
    public short MinutiInizio { get; set; }
    public short MinutiFine { get; set; }
    public bool DefaultSelected { get; set; }
    public bool DefaultSelectedWeb { get; set; }
    public string? PeriodoDelGiornoID { get; set; }
}

public class FilialeRiposoSettimanale
{
    public short FilialeRiposoSettimanaleID { get; set; }
    public int FilialeID { get; set; }
    public byte GiornoSettimana { get; set; }
}

public class FilialeVal
{
    public int FilialeVALID { get; set; }
    public int FilialeID { get; set; }
}

// PK: CodiceMezzo varchar(20) — non un int
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

// PK: CodiceSegmento varchar(3) — non un int
public class SegmentoModello
{
    public string CodiceSegmento { get; set; } = string.Empty;
    public string CodiceCategoria { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public byte Ordinamento { get; set; }
    public int? ModelloMezzoIDPdfOfferta { get; set; }
    public byte? Stato { get; set; }
    public int? ModelloMezzoIDErs { get; set; }
    public string? FleetID { get; set; }
    public int? SegmentoModelloClasseID { get; set; }
    public short? IndexPricing { get; set; }
    public int? ListinoID { get; set; }
    public decimal? ImportoVAL { get; set; }
    public DateTime? DataInserimento { get; set; }
    public DateTime? DataUltimaModifica { get; set; }
}

public class Preventivo
{
    public int PreventivoID { get; set; }
}

public class WsTokenPreventivo
{
    public int WsTokenPreventivoID { get; set; }
    public int PreventivoID { get; set; }
}

public class Listino
{
    public int ListinoID { get; set; }
}

public class ListinoBrand
{
    public int ListinoBrandID { get; set; }
    public int ListinoID { get; set; }
}

public class ListinoGiorni
{
    public int ListinoGiorniID { get; set; }
    public int ListinoID { get; set; }
}

public class ListinoKm
{
    public int ListinoKmID { get; set; }
    public int ListinoID { get; set; }
}

public class Km
{
    public int KmID { get; set; }
}

public class ListinoValori
{
    public int ListinoValoriID { get; set; }
    public int ListinoID { get; set; }
}

public class ListinoFranchigia
{
    public int ListinoFranchigiaID { get; set; }
    public int ListinoID { get; set; }
}

public class TipologiaFranchigia
{
    public int TipologiaFranchigiaID { get; set; }
}

public class AccordoCommercialeListino
{
    public int AccordoCommercialeListinoID { get; set; }
    public int ListinoID { get; set; }
}

public class RegolaDiVenditaListino
{
    public int RegolaDiVenditaListinoID { get; set; }
    public int ListinoID { get; set; }
}
