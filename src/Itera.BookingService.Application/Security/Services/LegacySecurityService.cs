using FluentValidation;
using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Contracts.Legacy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Itera.BookingService.Application.Security.Services;

public sealed class LegacySecurityService : ISecurityService
{
    private readonly ISecurityQueryService _query;
    private readonly IValidator<GetTokenRequest> _getTokenValidator;
    private readonly IValidator<ValidateTokenRequest> _validateTokenValidator;
    private readonly LegacyAuthOptions _authOptions;
    private readonly ILogger<LegacySecurityService> _logger;

    public LegacySecurityService(
        ISecurityQueryService query,
        IValidator<GetTokenRequest> getTokenValidator,
        IValidator<ValidateTokenRequest> validateTokenValidator,
        IOptions<LegacyAuthOptions> authOptions,
        ILogger<LegacySecurityService> logger)
    {
        _query = query;
        _getTokenValidator = getTokenValidator;
        _validateTokenValidator = validateTokenValidator;
        _authOptions = authOptions.Value;
        _logger = logger;
    }

    public async Task<ApiResponse<WsAuth>> GetTokenAsync(
        GetTokenRequest request, CancellationToken ct = default)
    {
        var validation = await _getTokenValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            _logger.LogWarning("GetToken validazione fallita Username={Username}", request.Username);
            return new ApiResponse<WsAuth>
            {
                Esito = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)),
                Data = null
            };
        }

        var user = await _query.ValidateUserAsync(request.Username, request.Password, ct);
        if (user is null)
        {
            _logger.LogWarning("GetToken credenziali non valide Username={Username}", request.Username);
            return new ApiResponse<WsAuth>
            {
                Esito = false,
                CodiceErrore = "INVALID_LOGIN",
                Messaggio = "Username o password non validi.",
                Data = new WsAuth(null)
            };
        }

        var token = await _query.CheckOrCreateTokenAsync(
            user.Value.WsUserID, user.Value.BrandID, _authOptions.TokenValidPeriodHours, ct);

        if (token is null)
        {
            _logger.LogError("GetToken generazione token fallita WsUserID={WsUserID}", user.Value.WsUserID);
            return new ApiResponse<WsAuth>
            {
                Esito = false,
                CodiceErrore = "TOKEN_GENERATION_ERROR",
                Messaggio = "Impossibile generare un token nuovo.",
                Data = new WsAuth(null)
            };
        }

        _logger.LogInformation("GetToken completato WsUserID={WsUserID} BrandID={BrandID}",
            user.Value.WsUserID, user.Value.BrandID);

        return ApiResponse<WsAuth>.Ok(new WsAuth(token.Value.ToString()));
    }

    public async Task<ApiResponse<object?>> ValidateTokenAsync(
        ValidateTokenRequest request, CancellationToken ct = default)
    {
        var validation = await _validateTokenValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return new ApiResponse<object?>
            {
                Esito = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))
            };

        var tokenGuid = Guid.Parse(request.Token);
        var brandId = await _query.ValidateTokenAsync(tokenGuid, _authOptions.TokenValidPeriodHours, ct);

        if (brandId is null)
        {
            _logger.LogWarning("ValidateToken non valido o scaduto Token={Token}", request.Token);
            return new ApiResponse<object?>
            {
                Esito = false,
                CodiceErrore = "INVALID_TOKEN",
                Messaggio = "Token scaduto o non valido."
            };
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