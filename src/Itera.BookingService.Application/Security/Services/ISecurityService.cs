using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Contracts.Legacy;

namespace Itera.BookingService.Application.Security.Services;

public interface ISecurityService
{
    Task<WsResponse<LoginResponse>> LoginAsync(
        LoginRequest request, CancellationToken ct = default);

    Task<WsResponse<GetUserInfoResponse>> GetUserInfoAsync(
        GetUserInfoRequest request,
        LegacyAuthContext auth,
        CancellationToken ct = default);
}