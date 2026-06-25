using Itera.BookingService.Contracts.Legacy.Branch;

namespace Itera.BookingService.Application.Abstractions;

public interface IBranchInfoQueryService
{
    Task<List<WsFiliale>> GetAllBranchesAsync(short brandId, bool getExtraData, bool getFilialiExtra, byte languageId, DateTime selectedDate, CancellationToken cancellationToken);
    Task<WsFiliale?> GetInfoBranchAsync(short brandId, int branchId, bool getFilialiExtra, byte languageId, DateTime selectedDate, CancellationToken cancellationToken);
}
