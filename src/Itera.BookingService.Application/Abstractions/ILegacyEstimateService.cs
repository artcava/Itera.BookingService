using Itera.BookingService.Contracts.Legacy;
using Itera.BookingService.Contracts.Legacy.Estimate;

namespace Itera.BookingService.Application.Abstractions;

public interface ILegacyEstimateService
{
    Task<ApiResponse<List<Categoria>>> GetAllCategoryAsync(
        GetAllCategorieRequest request,
        LegacyAuthContext      authContext,
        CancellationToken      cancellationToken);

    Task<ApiResponse<List<KmOpzione>>> GetKmsAsync(
        GetKmsRequest     request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken);

    Task<ApiResponse<GetDefaultValues>> GetDefaultValuesAsync(
        GetDefaultValuesRequest request,
        LegacyAuthContext       authContext,
        CancellationToken       ct);

    Task<ApiResponse<List<GetProvince>>> GetProvinceAsync(
        GetProvinceRequest request,
        LegacyAuthContext  authContext,
        CancellationToken  ct);
}
