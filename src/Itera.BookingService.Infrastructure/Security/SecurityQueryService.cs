using Itera.BookingService.Application.Security;
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

    public async Task<(int WsUserID, short BrandID)?> ValidateUserAsync(
        string username, string secretWord, CancellationToken ct)
    {
        var user = await _db.WsUsers
            .AsNoTracking()
            .Where(u => u.Username == username && u.SecretWord == secretWord && u.BrandID != null)
            .Select(u => new { u.WsUserID, u.BrandID })
            .FirstOrDefaultAsync(ct);

        if (user is null)
        {
            _logger.LogWarning("ValidateUser: nessun utente trovato per Username={Username}", username);
            return null;
        }

        return (user.WsUserID, user.BrandID!.Value);
    }

    public async Task<Guid?> CheckOrCreateTokenAsync(
        int wsUserID, short brandID, int tokenValidPeriodHours, CancellationToken ct)
    {
        var cutoff = DateTime.UtcNow.AddHours(-tokenValidPeriodHours);

        var existing = await _db.WsTokens
            .Where(t => t.WsUserID == wsUserID
                     && t.BrandID == brandID
                     && t.DataUltimaModifica >= cutoff)
            .OrderByDescending(t => t.DataUltimaModifica)
            .Select(t => t.Token)
            .FirstOrDefaultAsync(ct);

        if (existing != default)
            return existing;

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
            return newToken.Token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "CheckOrCreateToken: errore salvataggio WsToken WsUserID={WsUserID}", wsUserID);
            return null;
        }
    }

    public async Task<short?> ValidateTokenAsync(
        Guid token, int tokenValidPeriodHours, CancellationToken ct)
    {
        var cutoff = DateTime.UtcNow.AddHours(-tokenValidPeriodHours);

        var record = await _db.WsTokens
            .AsNoTracking()
            .Where(t => t.Token == token && t.DataUltimaModifica >= cutoff)
            .Select(t => (short?)t.BrandID)
            .FirstOrDefaultAsync(ct);

        return record;
    }
}