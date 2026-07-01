using FluentValidation;
using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Validators;

public sealed class GetDefaultValuesRequestValidator : AbstractValidator<GetDefaultValuesRequest>
{
    public GetDefaultValuesRequestValidator()
    {
        RuleFor(x => x.BrandID)
            .GreaterThan((short)0)
            .WithMessage("BrandID non valido");

        RuleFor(x => x.BranchID)
            .GreaterThan(0)
            .When(x => x.BranchID.HasValue)
            .WithMessage("BranchID non valido");
    }
}
