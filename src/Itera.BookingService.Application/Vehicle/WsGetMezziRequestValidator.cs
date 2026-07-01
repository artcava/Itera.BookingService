using FluentValidation;
using Itera.BookingService.Contracts.Vehicle;

namespace Itera.BookingService.Application.Vehicle;

public sealed class GetMezziRequestValidator : AbstractValidator<GetMezziRequest>
{
    public GetMezziRequestValidator()
    {
        RuleFor(x => x.GruppoID)
            .GreaterThan(0)
            .When(x => x.GruppoID.HasValue)
            .WithMessage("GruppoID deve essere maggiore di zero");
    }
}
