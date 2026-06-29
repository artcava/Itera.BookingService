using FluentValidation;
using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Application.Shared;
using Itera.BookingService.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Application.Security.Services;

public sealed class LegacySecurityService : ISecurityService
{
    private readonly ISecurityQueryService _query;
    private readonly IValidator<GetTokenRequest> _getTokenValidator;
    private readonly IValidator<ValidateTokenRequest> _validateTokenValidator;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LegacySecurityService> _logger;

    public LegacySecurityService(
        ISecurityQueryService query,
        IValidator<GetTokenRequest> getTokenValidator,
        IValidator<ValidateTokenRequest> validateTokenValidator,
        IConfiguration configuration,
        ILogger<LegacySecurityService> logger)
    {
        _query = query;
        _getTokenValidator = getTokenValidator;
        _validateTokenValidator = validateTokenValidator;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<WsResponse<WsAuth>> GetTokenAsync(
        GetTokenRequest request, CancellationToken ct = default)
    {
        var validation = await _getTokenValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            _logger.LogWarning("GetToken validazione fallita Username={Username}", request.Username);
            return WsResponse<WsAuth>.ValidationError(validation);
        }

        var userResult = await _query.ValidateUserAsync(request.Username, request.Password, ct);
        if (!userResult.IsSuccess)
        {
            _logger.LogWarning("GetToken credenziali non valide Username={Username}", request.Username);
            return WsResponse<WsAuth>.Fail(userResult.Error);
        }

        var (wsUserID, brandID) = userResult.Value;
        int tokenHours = _configuration.GetValue<int?>("Security:TokenValidPeriodHours") ?? 24;

        var tokenResult = await _query.CheckOrCreateTokenAsync(wsUserID, brandID, tokenHours, ct);
        if (!tokenResult.IsSuccess)
        {
            _logger.LogError("GetToken generazione token fallita WsUserID={WsUserID}", wsUserID);
            return WsResponse<WsAuth>.Fail(tokenResult.Error);
        }

        _logger.LogInformation("GetToken completato WsUserID={WsUserID} BrandID={BrandID}", wsUserID, brandID);
        return WsResponse<WsAuth>.Ok(new WsAuth(tokenResult.Value.ToString()));
    }

    public async Task<WsResponse<object>> ValidateTokenAsync(
        ValidateTokenRequest request, CancellationToken ct = default)
    {
        var validation = await _validateTokenValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return WsResponse<object>.ValidationError(validation);

        var tokenGuid = Guid.Parse(request.Token);
        int tokenHours = _configuration.GetValue<int?>("Security:TokenValidPeriodHours") ?? 24;

        var result = await _query.ValidateTokenAsync(tokenGuid, tokenHours, ct);
        if (!result.IsSuccess)
        {
            _logger.LogWarning("ValidateToken token non valido o scaduto Token={Token}", request.Token);
            return WsResponse<object>.Fail(result.Error);
        }

        return WsResponse<object>.Ok(null);
    }

    public Task<WsResponse<object>> ResetKeyCacheAsync(
        ResetKeyCacheRequest request, CancellationToken ct = default)
    {
        // La cache SQL del legacy (CacheHelper.EliminaTagCacheQuery) non ha equivalente
        // in .NET 10. L'operazione è un no-op documentato: i client che la invocano
        // ricevono risposta di successo senza side-effect.
        _logger.LogInformation(
            "ResetKeyCache invocato (no-op in .NET 10) KeySqlCache={KeySqlCache}",
            request.KeySqlCache ?? "<null>");

        return Task.FromResult(WsResponse<object>.Ok(null));
    }
}