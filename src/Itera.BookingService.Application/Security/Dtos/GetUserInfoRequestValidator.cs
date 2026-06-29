using FluentValidation;
using Itera.BookingService.Application.Security.Dtos;

namespace Itera.BookingService.Application.Security.Validators;

public sealed class GetUserInfoRequestValidator : AbstractValidator<GetUserInfoRequest>
{
    public GetUserInfoRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId obbligatorio.");
    }
}