using FluentValidation;
using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Validators;

public sealed class GetWholeEstimateRequestValidator : AbstractValidator<GetWholeEstimateRequest>
{
    public GetWholeEstimateRequestValidator()
    {
        RuleFor(x => x.EstimateToken)
            .NotEmpty()
            .WithMessage("EstimateToken è obbligatorio")
            .Must(static token => Guid.TryParse(token, out _))
            .WithMessage("EstimateToken non valido");
    }
}