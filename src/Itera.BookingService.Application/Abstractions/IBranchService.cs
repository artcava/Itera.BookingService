using Itera.BookingService.Contracts.General;
using Itera.BookingService.Contracts.Branch;

namespace Itera.BookingService.Application.Abstractions;

public interface IBranchService
{
    Task<ApiResponse<List<FilialeDto>>> GetAllBranchesAsync(GetAllBranchesRequest request, LegacyAuthContext authContext, CancellationToken cancellationToken);
    Task<ApiResponse<FilialeDto?>> GetInfoBranchAsync(GetBranchInfoRequest request, LegacyAuthContext authContext, CancellationToken cancellationToken);
}
