namespace Itera.BookingService.Infrastructure.Persistence;

public class GetFilialeInfoWs2Result
{
    public int? FilialeID { get; set; }
    public string? Descrizione { get; set; }
    public int? FranchiseID { get; set; }
    public string? Indirizzo { get; set; }
    public string? Cap { get; set; }
    public string? Citta { get; set; }
    public string? Telefono { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? OrariUfficioDescrizione { get; set; }
    public string? OrariConsegnaDescrizione { get; set; }
    public double? CoordX { get; set; }
    public double? CoordY { get; set; }
    public double? SpnX { get; set; }
    public double? SpnY { get; set; }
    public short? Stato { get; set; }
    public string? Parcheggio { get; set; }
    public string? RespCommerciale { get; set; }
    public string? RespAmministrazione { get; set; }
    public bool? KeyBox { get; set; }
    public int? FilialeAreaID { get; set; }
    public int? FilialeMacroAreaID { get; set; }
    public bool? EsclusioneVal { get; set; }
    public int? ClassificazioneID { get; set; }
    public string? Provincia { get; set; }
    public string? Regione { get; set; }
}

public class GetFilialiFatturazioneClienteWsResult
{
    public int? FilialeID { get; set; }
}

// Risultato della SP [dbo].[GetMezziWs] — campi derivati da ModelloMezzo, SegmentoModello, AlimentazioneModello, Marca
public class GetMezziWsResult
{
    public int? ModelloMezzoID { get; set; }
    public string? Marca { get; set; }
    public string? ModelloDescr { get; set; }
    public string? NomeImmagine { get; set; }
    public string? Cilindrata { get; set; }
    public int? AlimentazioneModelloID { get; set; }
    public string? AlimentazioneDescr { get; set; }
    public string? Euro { get; set; }
    public short? NumeroPosti { get; set; }
    public short? NumeroPorte { get; set; }
    public bool? Autoradio { get; set; }
    public bool? AriaCondizionata { get; set; }
    public bool? Abs { get; set; }
    public short? Airbag { get; set; }
    public short? CapacitaSerbatoio { get; set; }
    public int? Portata { get; set; }
    public int? VolumeCarico { get; set; }
    public int? AltezzaInterna { get; set; }
    public int? LarghezzaInterna { get; set; }
    public int? LunghezzaInterna { get; set; }
    public string? SegmentoDescrizione { get; set; }
    public string? CodiceSegmento { get; set; }
    public int? ModelloMezzoIDErs { get; set; }
    public short? IndexPricing { get; set; }
    public int? SegmentoModelloClasseID { get; set; }
    public string? SegmentoModelloClasseIDDescrizione { get; set; }
    public int? AltezzaEsterna { get; set; }
    public int? LarghezzaEsterna { get; set; }
    public int? LunghezzaEsterna { get; set; }
    public int? Passo { get; set; }
    public int? LarghezzaPassaruote { get; set; }
    public short? NumeroPostiCarrozzina { get; set; }
    public short? NumeroPostiMobility { get; set; }
    public bool? PedanaSollevatriceDoppioBraccio { get; set; }
    public string? DescrizioneMobilitySitoWeb_ITA { get; set; }
    public string? DescrizioneMobilitySitoWeb_ENG { get; set; }
}

public class GetListinoValoriResult
{
    public int? ListinoID { get; set; }
}

public class GetKmInfoMultiResult
{
    public int? KmID { get; set; }
}

public class VwListinoFranchigiaResult
{
    public int? ListinoFranchigiaID { get; set; }
}
