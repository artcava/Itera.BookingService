using FluentValidation;
using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Validators;

public sealed class GetInsuranceExtraRequestValidator : AbstractValidator<GetInsuranceExtraRequest>
{
    public GetInsuranceExtraRequestValidator()
    {
        RuleFor(x => x.RentalDays)
            .GreaterThan(0)
            .WithMessage("RentalDays deve essere maggiore di zero");

        RuleFor(x => x.DateFrom)
            .NotEmpty()
            .WithMessage("DateFrom è obbligatorio");

        RuleFor(x => x.DateTo)
            .NotEmpty()
            .WithMessage("DateTo è obbligatorio");
    }
}