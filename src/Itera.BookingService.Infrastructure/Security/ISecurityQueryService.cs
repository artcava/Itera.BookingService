using Itera.BookingService.Application.Security.Dtos;
using Itera.BookingService.Application.Shared;

namespace Itera.BookingService.Infrastructure.Security;

public interface ISecurityQueryService
{
    Task<Result<LoginResponse>> AuthenticateAsync(LoginRequest request, CancellationToken ct);
    Task<Result<WsUserDto>> GetUserInfoAsync(string userId, CancellationToken ct);
}