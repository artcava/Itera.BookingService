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
