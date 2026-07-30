using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Abstractions;

public interface IEstimateQueryService
{
    Task<List<InsuranceExtraDto>> GetInsuranceExtraAsync(
        string segmentCode,
        DateTime dateFrom,
        DateTime dateTo,
        int rentalDays,
        int catalogId,
        CancellationToken cancellationToken);
    Task<EstimateTokenValidationResult> ValidateEstimateTokenAsync(
        string estimateToken,
        int tokenValidPeriodSeconds,
        CancellationToken cancellationToken);

    Task<bool> AcceptsNonSellableSegmentAsync(
        int wsUserId,
        CancellationToken cancellationToken);

    Task<Dictionary<short, string>> GetAccessoryCodesByIdsAsync(
        IReadOnlyCollection<short> accessoryIds,
        CancellationToken cancellationToken);

    Task<List<EstimateInsuranceOption>> GetInsuranceOptionsAsync(
        string segmentCode,
        int rentalDays,
        int catalogId,
        DateTime dateFrom,
        DateTime dateTo,
        CancellationToken cancellationToken);

    Task<short?> GetCurrentIvaIdAsync(CancellationToken cancellationToken);

    Task<decimal?> GetCurrentIvaPercentageAsync(CancellationToken cancellationToken);

    Task<List<AccessoryBookingDto>> GetAccessoryBookingAsync(
        short brandId,
        int branchId,
        int branchDestinationId,
        int catalogId,
        int rentalDays,
        DateTime dateFrom,
        DateTime dateTo,
        string? categoryId,
        string? segmentCode,
        CancellationToken cancellationToken);

    Task<EstimateDto> GetWholeEstimateAsync(
        short brandId,
        string estimateToken,
        string? segmentCode,
        string? KmType,
        IReadOnlyCollection<int>? InsuranceExtraList,
        IReadOnlyCollection<InsuranceRequest>? InsuranceList,
        IReadOnlyCollection<AccessoryRequest>? AccessoryList,
        string? BookingCode,
        bool Prepaid,
        CancellationToken cancellationToken);
}
public sealed record EstimateTokenValidationResult(
    int ValidationCode,
    EstimateTokenSnapshot? Snapshot);

public sealed record EstimateTokenSnapshot(
    int WsUserId,
    int FilialeId,
    int FilialeDestinazioneId,
    DateTime DataFromPreventivo,
    DateTime DataToPreventivo,
    int? Giorni,
    int? ListinoId,
    string? CodiceDurata,
    string? CodiceCategoria,
    string? ObjectDynParam,
    string? ObjectEstimate,
    string? VoucherCliente);

public sealed record EstimateInsuranceOption(
    int ListinoFranchigiaId,
    string TipologiaFranchigiaId);