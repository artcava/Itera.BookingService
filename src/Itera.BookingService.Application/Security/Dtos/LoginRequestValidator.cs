using FluentValidation;
using Itera.BookingService.Application.Security.Dtos;

namespace Itera.BookingService.Application.Security.Validators;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username obbligatorio.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password obbligatoria.");

        RuleFor(x => x.CodiceFiliale)
            .NotEmpty().WithMessage("CodiceFiliale obbligatorio.");
    }
}