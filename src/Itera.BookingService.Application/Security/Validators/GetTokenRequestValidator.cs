using FluentValidation;
using Itera.BookingService.Application.Security.Dtos;

namespace Itera.BookingService.Application.Security.Validators;

public sealed class GetTokenRequestValidator : AbstractValidator<GetTokenRequest>
{
    public GetTokenRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username obbligatorio.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password obbligatoria.");
    }
}