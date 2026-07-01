using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Estimate;

namespace Itera.BookingService.Application.Abstractions;

public interface ILegacyEstimateService
{
    Task<WsResponse<List<WsCategoria>>> GetAllCategoryAsync(
        WsGetAllCategorieRequest request,
        LegacyAuthContext        authContext,
        CancellationToken        cancellationToken);

    Task<WsResponse<List<WsKmOpzione>>> GetKmsAsync(
        WsGetKmsRequest  request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken);
}
