namespace Itera.BookingService.Application.Abstractions;

public interface ITokenValidationService
{
    Task<TokenValidationResult> ValidateAsync(string token, int tokenValidPeriodHours, CancellationToken cancellationToken);
}

public sealed class TokenValidationResult
{
    public bool IsValid { get; init; }
    public int ErrorCode { get; init; }
    public int WsUserId { get; init; }
    public short BrandId { get; init; }
}
