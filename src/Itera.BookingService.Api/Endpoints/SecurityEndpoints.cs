using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Application.Security.Services;
using Itera.BookingService.Contracts.General;
using Microsoft.AspNetCore.Mvc;

namespace Itera.BookingService.Api.Endpoints;

public static class SecurityEndpoints
{
    public static IEndpointRouteBuilder MapSecurityEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/SecurityService.svc/GetToken",
            async (
                [FromBody] GetTokenRequest request,
                ISecurityService svc,
                CancellationToken ct) =>
            {
                var response = await svc.GetTokenAsync(request, ct);
                return Results.Ok(response);
            })
            .WithName("Security_GetToken")
            .WithTags("Security")
            .Produces<ApiResponse<AuthTokenData>>()
            .AllowAnonymous();

        app.MapPost("/SecurityService.svc/ValidateToken",
            async (
                [FromBody] ValidateTokenRequest request,
                ISecurityService svc,
                CancellationToken ct) =>
            {
                var response = await svc.ValidateTokenAsync(request, ct);
                return Results.Ok(response);
            })
            .WithName("Security_ValidateToken")
            .WithTags("Security")
            .Produces<ApiResponse<object?>>()
            .AllowAnonymous();

        app.MapPost("/SecurityService.svc/ResetKeyCache",
            async (
                [FromBody] ResetKeyCacheRequest request,
                ISecurityService svc,
                CancellationToken ct) =>
            {
                var response = await svc.ResetKeyCacheAsync(request, ct);
                return Results.Ok(response);
            })
            .WithName("Security_ResetKeyCache")
            .WithTags("Security")
            .Produces<ApiResponse<object?>>()
            .AddEndpointFilter<LegacyTokenEndpointFilter>();

        return app;
    }
}
