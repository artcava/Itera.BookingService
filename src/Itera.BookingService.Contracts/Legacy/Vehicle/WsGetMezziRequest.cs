namespace Itera.BookingService.Contracts.Legacy.Vehicle;

public sealed class WsGetMezziRequest
{
    public string? Token { get; set; }

    /// <summary>CSV di FleetID (char(1)), es. "A,B". NULL = tutti.</summary>
    public string? FleetMulti { get; set; }

    /// <summary>CSV di CodiceSegmento (varchar(3)), es. "ECO,MID". NULL = tutti.</summary>
    public string? SegmentoMulti { get; set; }

    /// <summary>true = solo speciali, false = solo standard, null = entrambi.</summary>
    public bool? MezzoSpeciale { get; set; }

    /// <summary>Filtra per GruppoID in ModelloMezzoGruppo. NULL = nessun filtro.</summary>
    public int? GruppoID { get; set; }
}
