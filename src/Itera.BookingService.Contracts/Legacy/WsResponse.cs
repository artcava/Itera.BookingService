namespace Itera.BookingService.Contracts.Legacy;

public class WsResponse
{
    public bool Esito { get; init; }
    public string? Messaggio { get; init; }
    public string? CodiceErrore { get; init; }

    public static WsResponse Ok() => new() { Esito = true };

    public static WsResponse NotImplemented(string endpointName) => new()
    {
        Esito = false,
        CodiceErrore = "NOT_IMPLEMENTED",
        Messaggio = $"Endpoint '{endpointName}' non ancora migrato."
    };
}

public class WsResponse<T> : WsResponse
{
    public T? Data { get; init; }

    public static WsResponse<T> Ok(T? data) => new()
    {
        Esito = true,
        Data = data
    };

    public new static WsResponse<T> NotImplemented(string endpointName) => new()
    {
        Esito = false,
        CodiceErrore = "NOT_IMPLEMENTED",
        Messaggio = $"Endpoint '{endpointName}' non ancora migrato.",
        Data = default
    };
}
