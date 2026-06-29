using Itera.BookingService.Application.Shared;
using Itera.BookingService.Infrastructure.Persistence;
using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Infrastructure.Security;

public sealed class SecurityQueryService : ISecurityQueryService
{
    private readonly LegacyDbContext _db;
    private readonly ILogger<SecurityQueryService> _logger;

    public SecurityQueryService(LegacyDbContext db, ILogger<SecurityQueryService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Result<(int WsUserID, short BrandID)>> ValidateUserAsync(
        string username, string secretWord, CancellationToken ct)
    {
        // Il legacy usa SecretWord in chiaro (varchar 50) — nessun hash applicativo.
        // La validazione è una semplice query per Username + SecretWord.
        var user = await _db.WsUsers
            .AsNoTracking()
            .Where(u => u.Username == username && u.SecretWord == secretWord)
            .Select(u => new { u.WsUserID, u.BrandID })
            .FirstOrDefaultAsync(ct);

        if (user is null || user.BrandID is null)
        {
            _logger.LogWarning("ValidateUser fallito Username={Username}", username);
            return Result<(int, short)>.Failure(
                new ServiceError("INVALID_LOGIN", "Username o password non validi."));
        }

        return Result<(int, short)>.Success((user.WsUserID, user.BrandID.Value));
    }

    public async Task<Result<Guid>> CheckOrCreateTokenAsync(
        int wsUserID, short brandID, int tokenValidPeriodHours, CancellationToken ct)
    {
        var cutoff = DateTime.UtcNow.AddHours(-tokenValidPeriodHours);

        // Cerca token valido esistente per questo utente e brand
        var existing = await _db.WsTokens
            .Where(t => t.WsUserID == wsUserID
                     && t.BrandID == brandID
                     && t.DataUltimaModifica >= cutoff)
            .OrderByDescending(t => t.DataUltimaModifica)
            .FirstOrDefaultAsync(ct);

        if (existing is not null)
            return Result<Guid>.Success(existing.Token);

        // Crea nuovo token
        var newToken = new WsToken
        {
            WsUserID = wsUserID,
            BrandID = brandID,
            Token = Guid.NewGuid(),
            DataCreazione = DateTime.UtcNow,
            DataUltimaModifica = DateTime.UtcNow
        };

        _db.WsTokens.Add(newToken);

        try
        {
            await _db.SaveChangesAsync(ct);
            return Result<Guid>.Success(newToken.Token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "CheckOrCreateToken: errore salvataggio WsToken WsUserID={WsUserID}", wsUserID);
            return Result<Guid>.Failure(
                new ServiceError("TOKEN_GENERATION_ERROR", "Impossibile generare un token nuovo."));
        }
    }

    public async Task<Result<short>> ValidateTokenAsync(
        Guid token, int tokenValidPeriodHours, CancellationToken ct)
    {
        var cutoff = DateTime.UtcNow.AddHours(-tokenValidPeriodHours);

        var record = await _db.WsTokens
            .AsNoTracking()
            .Where(t => t.Token == token && t.DataUltimaModifica >= cutoff)
            .Select(t => new { t.BrandID })
            .FirstOrDefaultAsync(ct);

        if (record is null)
            return Result<short>.Failure(
                new ServiceError("INVALID_TOKEN", "Token scaduto o non valido."));

        return Result<short>.Success(record.BrandID);
    }
}