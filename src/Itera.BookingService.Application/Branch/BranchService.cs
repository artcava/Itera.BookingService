using FluentValidation;
using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.General;
using Itera.BookingService.Contracts.Branch;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Application.Branch;

public sealed class BranchService(
    IValidator<GetAllBranchesRequest> allBranchesValidator,
    IValidator<GetBranchInfoRequest> infoBranchValidator,
    IBranchInfoQueryService branchInfoQueryService,
    ILogger<BranchService> logger) : IBranchService
{
    public async Task<ApiResponse<List<FilialeDto>>> GetAllBranchesAsync(GetAllBranchesRequest request, LegacyAuthContext authContext, CancellationToken cancellationToken)
    {
        var validation = await allBranchesValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return new ApiResponse<List<FilialeDto>>
            {
                Esito = false,
                CodiceErrore = "VALIDATION_ERROR",
                Messaggio = validation.Errors.First().ErrorMessage,
                Data = []
            };
        }

        var linguaId = LegacyRequestCultureDateResolver.ResolveLinguaId(request.Language);
        var selectedDate = LegacyRequestCultureDateResolver.ResolveDateStartLegacy(request.DateStart, linguaId);

        var branches = await branchInfoQueryService.GetAllBranchesAsync(
            authContext.BrandId,
            request.GetExtraData,
            request.GetFilialiExtra ?? false,
            linguaId,
            selectedDate,
            cancellationToken);

        logger.LogInformation("GetAllBranches resolved {Count} rows for WsUserID {WsUserID}", branches.Count, authContext.WsUserId);
        return ApiResponse<List<FilialeDto>>.Ok(branches);
    }

    public async Task<ApiResponse<FilialeDto?>> GetInfoBranchAsync(GetBranchInfoRequest request, LegacyAuthContext authContext, CancellationToken cancellationToken)
    {
        var validation = await infoBranchValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return new ApiResponse<FilialeDto?>
            {
                Esito = false,
                CodiceErrore = ApiErrorCodes.InvalidFilialeId.ToString(),
                Messaggio = "Invalid BranchID parameter",
                Data = null
            };
        }

        var linguaId = LegacyRequestCultureDateResolver.ResolveLinguaId(request.Language);
        var selectedDate = LegacyRequestCultureDateResolver.ResolveDateStartLegacy(request.DateStart, linguaId);

        var result = await branchInfoQueryService.GetInfoBranchAsync(
            authContext.BrandId,
            request.BranchID,
            request.GetFilialiExtra ?? false,
            linguaId,
            selectedDate,
            cancellationToken);

        if (result is null)
        {
            return new ApiResponse<FilialeDto?>
            {
                Esito = false,
                CodiceErrore = ApiErrorCodes.FilialeNotFound.ToString(),
                Messaggio = "Filiale inesistente",
                Data = null
            };
        }

        logger.LogInformation("GetInfoBranch resolved for BranchID {BranchID} and WsUserID {WsUserID}", request.BranchID, authContext.WsUserId);
        return ApiResponse<FilialeDto?>.Ok(result);
    }
}
