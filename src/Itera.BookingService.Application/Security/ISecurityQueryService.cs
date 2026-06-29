namespace Itera.BookingService.Application.Security;

public interface ISecurityQueryService
{
    Task<(int WsUserID, short BrandID)?> ValidateUserAsync(
        string username, string secretWord, CancellationToken ct);

    Task<Guid?> CheckOrCreateTokenAsync(
        int wsUserID, short brandID, int tokenValidPeriodHours, CancellationToken ct);

    Task<short?> ValidateTokenAsync(
        Guid token, int tokenValidPeriodHours, CancellationToken ct);
}
