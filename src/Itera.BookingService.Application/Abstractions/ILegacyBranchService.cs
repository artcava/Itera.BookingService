using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Branch;

namespace Itera.BookingService.Application.Abstractions;

public interface ILegacyBranchService
{
    Task<WsResponse<List<WsFiliale>>> GetAllBranchesAsync(WsGetAllFilialiRequest request, LegacyAuthContext authContext, CancellationToken cancellationToken);
    Task<WsResponse<WsFiliale?>> GetInfoBranchAsync(WsGetFilialeInfoRequest request, LegacyAuthContext authContext, CancellationToken cancellationToken);
}
