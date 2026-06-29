using FluentValidation;
using Itera.BookingService.Application.Security.Dtos;

namespace Itera.BookingService.Application.Security.Validators;

public sealed class ValidateTokenRequestValidator : AbstractValidator<ValidateTokenRequest>
{
    public ValidateTokenRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token obbligatorio.")
            .Must(t => Guid.TryParse(t, out _))
            .WithMessage("Formato token non valido.");
    }
}