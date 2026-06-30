namespace Itera.BookingService.Infrastructure.Persistence.Entities;

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
