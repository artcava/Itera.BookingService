using FluentValidation;
using Itera.BookingService.Contracts.Legacy.Branch;

namespace Itera.BookingService.Application.Branch;

public sealed class GetAllBranchesRequestValidator : AbstractValidator<GetAllBranchesRequest>
{
    public GetAllBranchesRequestValidator()
    {
        // Intentionally permissive for legacy parity.
    }
}
