using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Contracts.Legacy;

namespace Itera.BookingService.Application.Security.Services;

public interface ISecurityService
{
    Task<WsResponse<WsAuth>> GetTokenAsync(GetTokenRequest request, CancellationToken ct = default);
    Task<WsResponse<object>> ValidateTokenAsync(ValidateTokenRequest request, CancellationToken ct = default);
    Task<WsResponse<object>> ResetKeyCacheAsync(ResetKeyCacheRequest request, CancellationToken ct = default);
}