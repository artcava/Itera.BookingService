using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Contracts.General;

namespace Itera.BookingService.Application.Abstractions;

public interface IEstimateService
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

    Task<ApiResponse<List<Nazione>>> GetNationsAsync(
        GetNationsRequest request,
        LegacyAuthContext authContext,
        CancellationToken ct);

    Task<ApiResponse<List<AccessoryBookingDto>>> GetAccessoryBookingAsync(
        GetAccessoryBookingRequest request,
        LegacyAuthContext authContext,
        CancellationToken cancellationToken);
}
