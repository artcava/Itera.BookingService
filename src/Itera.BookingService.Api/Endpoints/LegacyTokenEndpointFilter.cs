using System.Text.Json;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace Itera.BookingService.Api.Endpoints;

public sealed class LegacyTokenEndpointFilter(
    ITokenValidationService tokenValidationService,
    IOptions<LegacyAuthOptions> authOptions,
    ILogger<LegacyTokenEndpointFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;
        var headerToken = ResolveHeaderToken(httpContext, authOptions.Value.HeaderName);
        var payloadToken = authOptions.Value.EnablePayloadTokenFallback ? ResolvePayloadToken(context.Arguments) : null;

        if (!string.IsNullOrWhiteSpace(payloadToken) && !string.IsNullOrWhiteSpace(headerToken) && !string.Equals(payloadToken, headerToken, StringComparison.Ordinal))
        {
            logger.LogWarning("Token mismatch between payload and header {HeaderName}. Payload token takes precedence.", authOptions.Value.HeaderName);
        }

        var effectiveToken = !string.IsNullOrWhiteSpace(payloadToken) ? payloadToken : headerToken;
        if (string.IsNullOrWhiteSpace(effectiveToken))
        {
            return Results.Json(BuildInvalidTokenResponse(LegacyErrorCodes.InvalidToken));
        }

        var validation = await tokenValidationService.ValidateAsync(effectiveToken, authOptions.Value.TokenValidPeriodHours, httpContext.RequestAborted);
        if (!validation.IsValid)
        {
            return Results.Json(BuildInvalidTokenResponse(validation.ErrorCode));
        }

        httpContext.Items[LegacyAuthContext.ItemKey] = new LegacyAuthContext
        {
            Token = effectiveToken,
            WsUserId = validation.WsUserId,
            BrandId = validation.BrandId
        };

        return await next(context);
    }

    private static string? ResolveHeaderToken(HttpContext context, string headerName)
    {
        return context.Request.Headers.TryGetValue(headerName, out var values)
            ? values.FirstOrDefault()
            : null;
    }

    private static string? ResolvePayloadToken(IList<object?> arguments)
    {
        foreach (var argument in arguments)
        {
            if (argument is ILegacyTokenCarrier tokenCarrier)
            {
                return tokenCarrier.Token;
            }

            if (argument is JsonElement payload && payload.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in payload.EnumerateObject())
                {
                    if (string.Equals(property.Name, "Token", StringComparison.OrdinalIgnoreCase)
                        && property.Value.ValueKind == JsonValueKind.String)
                    {
                        return property.Value.GetString();
                    }
                }
            }
        }

        return null;
    }

    private static WsResponse<object?> BuildInvalidTokenResponse(int errorCode)
    {
        var message = errorCode == LegacyErrorCodes.ExpiredToken
            ? "Token expired"
            : "Invalid token";

        return new WsResponse<object?>
        {
            Esito = false,
            CodiceErrore = errorCode.ToString(),
            Messaggio = message,
            Data = null
        };
    }
}
