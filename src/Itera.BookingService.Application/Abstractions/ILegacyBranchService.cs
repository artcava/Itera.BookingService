using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Branch;

namespace Itera.BookingService.Application.Abstractions;

public interface ILegacyBranchService
{
    Task<ApiResponse<List<Filiale>>> GetAllBranchesAsync(GetAllBranchesRequest request, LegacyAuthContext authContext, CancellationToken cancellationToken);
    Task<ApiResponse<Filiale?>> GetInfoBranchAsync(GetBranchInfoRequest request, LegacyAuthContext authContext, CancellationToken cancellationToken);
}
