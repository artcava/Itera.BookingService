namespace Itera.BookingService.Contracts.Legacy.Estimate;

/// <summary>
/// Request per l'endpoint EstimateService.svc/GetKms.
/// Replica WsGetKmsRequest del legacy (extend WsRequest -> Token inline).
/// </summary>
public sealed class WsGetKmsRequest
{
    /// <summary>Token di autenticazione (validato da LegacyTokenEndpointFilter).</summary>
    public string? Token { get; set; }

    /// <summary>Identificativo della filiale.</summary>
    public int FilialeId { get; set; }

    /// <summary>Codice categoria veicolo.</summary>
    public string? CategoriaId { get; set; }

    /// <summary>Data/ora ritiro nel formato "yyyy-MM-ddTHH:mm:ss".</summary>
    public string? DataFrom { get; set; }

    /// <summary>Data/ora consegna nel formato "yyyy-MM-ddTHH:mm:ss".</summary>
    public string? DataTo { get; set; }

    /// <summary>ID fascia oraria di ritiro.</summary>
    public int FasciaOrarioRitiro { get; set; }

    /// <summary>ID fascia oraria di consegna.</summary>
    public int FasciaOrarioConsegna { get; set; }
}
