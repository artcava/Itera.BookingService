namespace Itera.BookingService.Application.Security;

public interface ISecurityQueryService
{
    /// <summary>
    /// Valida Username + SecretWord. Restituisce (WsUserID, BrandID) oppure null se non valido.
    /// </summary>
    Task<(int WsUserID, short BrandID)?> ValidateUserAsync(
        string username, string secretWord, CancellationToken ct);

    /// <summary>
    /// Cerca un WsToken valido per WsUserID+BrandID, o ne crea uno nuovo su DB.
    /// Restituisce il Guid del token, oppure null in caso di errore di persistenza.
    /// </summary>
    Task<Guid?> CheckOrCreateTokenAsync(
        int wsUserID, short brandID, int tokenValidPeriodHours, CancellationToken ct);

    /// <summary>
    /// Verifica che il token esista e non sia scaduto. Restituisce BrandID o null se non valido.
    /// </summary>
    Task<short?> ValidateTokenAsync(
        Guid token, int tokenValidPeriodHours, CancellationToken ct);
}