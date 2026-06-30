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
