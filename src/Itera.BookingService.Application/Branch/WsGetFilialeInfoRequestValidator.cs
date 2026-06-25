using FluentValidation;
using Itera.BookingService.Contracts.Legacy.Branch;

namespace Itera.BookingService.Application.Branch;

public class WsGetFilialeInfoRequestValidator : AbstractValidator<WsGetFilialeInfoRequest>
{
    public WsGetFilialeInfoRequestValidator()
    {
        RuleFor(x => x.BranchID)
            .GreaterThan(0)
            .WithMessage("Invalid BranchID parameter");
    }
}
