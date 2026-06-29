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
        // Login — pre-auth, NON applica LegacyTokenEndpointFilter
        app.MapPost("/SecurityService.svc/Login",
            async (
                [FromBody] LoginRequest request,
                ISecurityService securityService,
                CancellationToken ct) =>
            {
                var response = await securityService.LoginAsync(request, ct);
                return response.ToHttpResult();
            })
            .WithName("Security_Login")
            .WithTags("Security")
            .Produces<WsResponse<LoginResponse>>()
            .ProducesProblem(400)
            .AllowAnonymous();

        // GetUserInfo — richiede token valido
        app.MapPost("/SecurityService.svc/GetUserInfo",
            async (
                [FromBody] GetUserInfoRequest request,
                ISecurityService securityService,
                HttpContext httpContext,
                CancellationToken ct) =>
            {
                var auth = httpContext.GetLegacyAuthContext();
                var response = await securityService.GetUserInfoAsync(request, auth, ct);
                return response.ToHttpResult();
            })
            .WithName("Security_GetUserInfo")
            .WithTags("Security")
            .AddEndpointFilter<LegacyTokenEndpointFilter>()
            .Produces<WsResponse<GetUserInfoResponse>>()
            .ProducesProblem(400)
            .ProducesProblem(401);

        return app;
    }
}