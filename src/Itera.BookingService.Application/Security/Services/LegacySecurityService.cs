using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Helpers;
using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Contracts.General;
using Itera.BookingService.Contracts.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Itera.BookingService.Application.Security.Services;

public sealed class LegacySecurityService : ISecurityService
{
    private readonly ISecurityQueryService _query;
    private readonly IValidator<GetTokenRequest> _getTokenValidator;
    private readonly IValidator<ValidateTokenRequest> _validateTokenValidator;
    private readonly AuthOptions _authOptions;
    private readonly ILogger<LegacySecurityService> _logger;

    public LegacySecurityService(
        ISecurityQueryService query,
        IValidator<GetTokenRequest> getTokenValidator,
        IValidator<ValidateTokenRequest> validateTokenValidator,
        IOptions<AuthOptions> authOptions,
        ILogger<LegacySecurityService> logger)
    {
        _query = query;
        _getTokenValidator = getTokenValidator;
        _validateTokenValidator = validateTokenValidator;
        _authOptions = authOptions.Value;
        _logger = logger;
    }

    public async Task<ApiResponse<AuthTokenData>> GetTokenAsync(
        GetTokenRequest request, CancellationToken ct = default)
    {
        var validation = await _getTokenValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            _logger.LogWarning("GetToken validazione fallita Username={Username}", request.Username);
            return ResponseHelper.LegacyError<AuthTokenData>("VALIDATION_ERROR", string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));
        }

        var user = await _query.ValidateUserAsync(request.Username, request.Password, ct);
        if (user is null)
        {
            _logger.LogWarning("GetToken credenziali non valide Username={Username}", request.Username);
            return ResponseHelper.LegacyError<AuthTokenData>("INVALID_LOGIN", "Username o password non validi.");
        }

        var token = await _query.CheckOrCreateTokenAsync(
            user.Value.WsUserID, user.Value.BrandID, _authOptions.TokenValidPeriodHours, ct);

        if (token is null)
        {
            _logger.LogError("GetToken generazione token fallita WsUserID={WsUserID}", user.Value.WsUserID);
            return ResponseHelper.LegacyError<AuthTokenData>("TOKEN_GENERATION_ERROR", "Impossibile generare un token nuovo.");
        }

        _logger.LogInformation("GetToken completato WsUserID={WsUserID} BrandID={BrandID}",
            user.Value.WsUserID, user.Value.BrandID);

        return ApiResponse<AuthTokenData>.Ok(new AuthTokenData(token.Value.ToString()));
    }

    public async Task<ApiResponse<object?>> ValidateTokenAsync(
        ValidateTokenRequest request, CancellationToken ct = default)
    {
        var validation = await _validateTokenValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return ResponseHelper.LegacyError<object?>("VALIDATION_ERROR", string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        var tokenGuid = Guid.Parse(request.Token);
        var brandId = await _query.ValidateTokenAsync(tokenGuid, _authOptions.TokenValidPeriodHours, ct);

        if (brandId is null)
        {
            _logger.LogWarning("ValidateToken non valido o scaduto Token={Token}", request.Token);
            
            return ResponseHelper.LegacyError<object?>("INVALID_TOKEN", "Token scaduto o non valido.");
        }

        return ApiResponse<object?>.Ok(null);
    }

    public Task<ApiResponse<object?>> ResetKeyCacheAsync(
        ResetKeyCacheRequest request, CancellationToken ct = default)
    {
        _logger.LogInformation(
            "ResetKeyCache invocato (no-op in .NET 10) KeySqlCache={KeySqlCache}",
            request.KeySqlCache ?? "<null>");

        return Task.FromResult(ApiResponse<object?>.Ok(null));
    }
}
