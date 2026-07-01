using System.Text.Json.Serialization;

namespace Itera.BookingService.Contracts.General;

public class ApiResponse
{
    public bool Esito { get; init; }
    public string? Messaggio { get; init; }
    public string? CodiceErrore { get; init; }

    /// <summary>
    /// Dati aggiuntivi opzionali legati all'errore (es. NewDataTo per PeriodoSuperioreAlMese).
    /// Non serializzato se null.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? ErrorExtraData { get; init; }

    public static ApiResponse Ok() => new() { Esito = true };

    public static ApiResponse NotImplemented(string endpointName) => new()
    {
        Esito = false,
        CodiceErrore = "NOT_IMPLEMENTED",
        Messaggio = $"Endpoint '{endpointName}' non ancora migrato."
    };
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; init; }

    public static ApiResponse<T> Ok(T? data) => new()
    {
        Esito = true,
        Data = data
    };

    public new static ApiResponse<T> NotImplemented(string endpointName) => new()
    {
        Esito = false,
        CodiceErrore = "NOT_IMPLEMENTED",
        Messaggio = $"Endpoint '{endpointName}' non ancora migrato.",
        Data = default
    };
}
