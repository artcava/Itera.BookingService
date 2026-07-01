using FluentValidation;
using Itera.BookingService.Contracts.Legacy.Branch;

namespace Itera.BookingService.Application.Branch;

public sealed class GetBranchInfoRequestValidator : AbstractValidator<GetBranchInfoRequest>
{
    public GetBranchInfoRequestValidator()
    {
        RuleFor(x => x.BranchID)
            .GreaterThan(0)
            .WithMessage("Invalid BranchID parameter");
    }
}
