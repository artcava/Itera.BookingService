using Itera.BookingService.Application.Shared;
using Itera.BookingService.Infrastructure.Persistence.Entities;

namespace Itera.BookingService.Infrastructure.Security;

public interface ISecurityQueryService
{
    /// <summary>
    /// Valida username/SecretWord e restituisce WsUserID + BrandID se validi.
    /// </summary>
    Task<Result<(int WsUserID, short BrandID)>> ValidateUserAsync(
        string username, string secretWord, CancellationToken ct);

    /// <summary>
    /// Cerca un token valido per WsUserID+BrandID o ne crea uno nuovo persistendo su WsToken.
    /// </summary>
    Task<Result<Guid>> CheckOrCreateTokenAsync(
        int wsUserID, short brandID, int tokenValidPeriodHours, CancellationToken ct);

    /// <summary>
    /// Verifica che il token Guid esista e non sia scaduto.
    /// Restituisce il BrandID associato se valido.
    /// </summary>
    Task<Result<short>> ValidateTokenAsync(
        Guid token, int tokenValidPeriodHours, CancellationToken ct);
}