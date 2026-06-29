using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Application.Security.Services;
using Itera.BookingService.Application.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class SecurityEndpoints
{
    public static IEndpointRouteBuilder MapSecurityEndpoints(
        this IEndpointRouteBuilder app)
    {
        // GetToken — pre-auth, nessun LegacyTokenEndpointFilter
        app.MapPost("/SecurityService.svc/GetToken",
            async (
                [FromBody] GetTokenRequest request,
                ISecurityService svc,
                CancellationToken ct) =>
            {
                var response = await svc.GetTokenAsync(request, ct);
                return response.ToHttpResult();
            })
            .WithName("Security_GetToken")
            .WithTags("Security")
            .Produces<WsResponse<WsAuth>>()
            .ProducesProblem(400)
            .AllowAnonymous();

        // ValidateToken — pre-auth (valida il token in ingresso, non ne richiede uno)
        app.MapPost("/SecurityService.svc/ValidateToken",
            async (
                [FromBody] ValidateTokenRequest request,
                ISecurityService svc,
                CancellationToken ct) =>
            {
                var response = await svc.ValidateTokenAsync(request, ct);
                return response.ToHttpResult();
            })
            .WithName("Security_ValidateToken")
            .WithTags("Security")
            .Produces<WsResponse<object>>()
            .ProducesProblem(400)
            .AllowAnonymous();

        // ResetKeyCache — richiede token valido
        app.MapPost("/SecurityService.svc/ResetKeyCache",
            async (
                [FromBody] ResetKeyCacheRequest request,
                ISecurityService svc,
                CancellationToken ct) =>
            {
                var response = await svc.ResetKeyCacheAsync(request, ct);
                return response.ToHttpResult();
            })
            .WithName("Security_ResetKeyCache")
            .WithTags("Security")
            .AddEndpointFilter<LegacyTokenEndpointFilter>()
            .Produces<WsResponse<object>>()
            .ProducesProblem(401);

        return app;
    }
}