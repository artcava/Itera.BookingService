using System.Text.Json;

namespace Itera.BookingService.Application.Abstractions;

public interface ILegacyEndpointExecutor
{
    Task<object> ExecuteJsonAsync(string serviceName, string endpointName, JsonElement payload, CancellationToken cancellationToken);
    Task<string> ExecuteXmlAsync(string serviceName, string endpointName, string payload, CancellationToken cancellationToken);
}
