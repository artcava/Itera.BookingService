using System.Text.Json;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.General;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Infrastructure.Execution;

public class LegacyEndpointExecutor(ILogger<LegacyEndpointExecutor> logger) : ILegacyEndpointExecutor
{
    public Task<object> ExecuteJsonAsync(string serviceName, string endpointName, JsonElement payload, CancellationToken cancellationToken)
    {
        logger.LogInformation("Legacy endpoint placeholder called: {Service}.{Endpoint}", serviceName, endpointName);
        var response = ApiResponse<object?>.NotImplemented($"{serviceName}/{endpointName}");
        return Task.FromResult<object>(response);
    }

    public Task<string> ExecuteXmlAsync(string serviceName, string endpointName, string payload, CancellationToken cancellationToken)
    {
        logger.LogInformation("Legacy XML endpoint placeholder called: {Service}.{Endpoint}", serviceName, endpointName);
        var xml = $"<WsMultaStatoRisposta><Esito>false</Esito><CodiceErrore>NOT_IMPLEMENTED</CodiceErrore><Messaggio>Endpoint '{serviceName}/{endpointName}' non ancora migrato.</Messaggio></WsMultaStatoRisposta>";
        return Task.FromResult(xml);
    }
}
