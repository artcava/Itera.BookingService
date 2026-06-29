using FluentValidation;
using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Application.Shared;
using Itera.BookingService.Infrastructure.Security;
using Itera.BookingService.Contracts.Legacy;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Application.Security.Services;

public sealed class LegacySecurityService : ISecurityService
{
    private readonly ISecurityQueryService _query;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<GetUserInfoRequest> _userInfoValidator;
    private readonly ILogger<LegacySecurityService> _logger;

    public LegacySecurityService(
        ISecurityQueryService query,
        IValidator<LoginRequest> loginValidator,
        IValidator<GetUserInfoRequest> userInfoValidator,
        ILogger<LegacySecurityService> logger)
    {
        _query = query;
        _loginValidator = loginValidator;
        _userInfoValidator = userInfoValidator;
        _logger = logger;
    }

    public async Task<WsResponse<LoginResponse>> LoginAsync(
        LoginRequest request, CancellationToken ct = default)
    {
        var validation = await _loginValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            _logger.LogWarning("Login validazione fallita per Username={Username}", request.Username);
            return WsResponse<LoginResponse>.ValidationError(validation);
        }

        _logger.LogInformation("Login richiesto per Username={Username} Filiale={Filiale}",
            request.Username, request.CodiceFiliale);

        var result = await _query.AuthenticateAsync(request, ct);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Login fallito per Username={Username} Errore={Errore}",
                request.Username, result.Error.Message);
            return WsResponse<LoginResponse>.Fail(result.Error);
        }

        return WsResponse<LoginResponse>.Ok(result.Value!);
    }

    public async Task<WsResponse<GetUserInfoResponse>> GetUserInfoAsync(
        GetUserInfoRequest request,
        LegacyAuthContext auth,
        CancellationToken ct = default)
    {
        var validation = await _userInfoValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            _logger.LogWarning("GetUserInfo validazione fallita UserId={UserId}", request.UserId);
            return WsResponse<GetUserInfoResponse>.ValidationError(validation);
        }

        _logger.LogInformation("GetUserInfo per UserId={UserId} richiesto da {RequesterId}",
            request.UserId, auth.UserId);

        var result = await _query.GetUserInfoAsync(request.UserId, ct);

        if (!result.IsSuccess)
            return WsResponse<GetUserInfoResponse>.Fail(result.Error);

        return WsResponse<GetUserInfoResponse>.Ok(new GetUserInfoResponse(result.Value!));
    }
}