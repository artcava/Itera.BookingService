using FluentValidation;
using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Validators;

public sealed class GetAccessoryBookingRequestValidator : AbstractValidator<GetAccessoryBookingRequest>
{
    public GetAccessoryBookingRequestValidator()
    {
        RuleFor(x => x.BranchId)
            .GreaterThan(0)
            .WithMessage("BranchID non valido");

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