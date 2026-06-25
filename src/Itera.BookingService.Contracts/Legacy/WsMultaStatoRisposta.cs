namespace Itera.BookingService.Contracts.Legacy;

public class WsMultaStatoRisposta
{
    public bool Esito { get; init; }
    public string? Messaggio { get; init; }
    public string? CodiceErrore { get; init; }
}

public class WsMultaStatoRisposta<T> : WsMultaStatoRisposta
{
    public T? Data { get; init; }
}
