using FluentValidation;
using Itera.BookingService.Contracts.Legacy.Vehicle;

namespace Itera.BookingService.Application.Vehicle;

public sealed class WsGetMezziRequestValidator : AbstractValidator<WsGetMezziRequest>
{
    public WsGetMezziRequestValidator()
    {
        RuleFor(x => x.GruppoID)
            .GreaterThan(0)
            .When(x => x.GruppoID.HasValue)
            .WithMessage("GruppoID deve essere maggiore di zero");
    }
}
