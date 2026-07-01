using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Contracts.Legacy;

namespace Itera.BookingService.Application.Security.Services;

public interface ISecurityService
{
    Task<ApiResponse<AuthTokenData>> GetTokenAsync(GetTokenRequest request, CancellationToken ct = default);
    Task<ApiResponse<object?>> ValidateTokenAsync(ValidateTokenRequest request, CancellationToken ct = default);
    Task<ApiResponse<object?>> ResetKeyCacheAsync(ResetKeyCacheRequest request, CancellationToken ct = default);
}
