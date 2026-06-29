using System.Text.Json;

namespace Itera.BookingService.Application.Abstractions;

/// <summary>
/// Placeholder per endpoint non ancora migrati. Restituisce WsResponse.NotImplemented per tutti i metodi.
/// Rimosso progressivamente man mano che ogni service viene migrato al pattern tipizzato (ILegacyXxxService).
/// Rimozione definitiva pianificata in Issue #9 (cleanup finale).
/// </summary>
[Obsolete("Placeholder temporaneo — non usare per endpoint in produzione. Vedi Issue #9 per la rimozione definitiva.")]
public interface ILegacyEndpointExecutor
{
    Task<object> ExecuteJsonAsync(string serviceName, string endpointName, JsonElement payload, CancellationToken cancellationToken);
    Task<string> ExecuteXmlAsync(string serviceName, string endpointName, string payload, CancellationToken cancellationToken);
}
