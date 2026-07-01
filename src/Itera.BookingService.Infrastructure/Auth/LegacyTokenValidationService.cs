using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.General;
using Itera.BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Itera.BookingService.Infrastructure.Auth;

public sealed class LegacyTokenValidationService(LegacyDbContext dbContext) : ITokenValidationService
{
    public async Task<TokenValidationResult> ValidateAsync(string token, int tokenValidPeriodHours, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(token, out var tokenGuid))
        {
            return Invalid(ApiErrorCodes.InvalidToken);
        }

        var tokenRow = await dbContext.WsTokens.FirstOrDefaultAsync(x => x.Token == tokenGuid, cancellationToken);
        if (tokenRow is null)
        {
            return Invalid(ApiErrorCodes.InvalidToken);
        }

        var referenceDate = tokenRow.DataUltimaModifica ?? tokenRow.DataCreazione;
        if (!referenceDate.HasValue || referenceDate.Value.AddHours(tokenValidPeriodHours) < DateTime.Now)
        {
            return Invalid(ApiErrorCodes.ExpiredToken);
        }

        tokenRow.DataUltimaModifica = DateTime.Now;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new TokenValidationResult
        {
            IsValid = true,
            ErrorCode = ApiErrorCodes.Success,
            WsUserId = tokenRow.WsUserID,
            BrandId = tokenRow.BrandID
        };
    }

    private static TokenValidationResult Invalid(int errorCode)
    {
        return new TokenValidationResult
        {
            IsValid = false,
            ErrorCode = errorCode,
            WsUserId = 0,
            BrandId = 0
        };
    }
}
