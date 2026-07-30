using FluentValidation;
using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Validators;

public sealed class GetAmountEstimateRequestValidator : AbstractValidator<GetAmountEstimateRequest>
{
    public GetAmountEstimateRequestValidator()
    {
        RuleFor(x => x.EstimateToken)
            .NotEmpty()
            .WithMessage("EstimateToken è obbligatorio")
            .Must(static token => Guid.TryParse(token, out _))
            .WithMessage("EstimateToken non valido");

        RuleFor(x => x.SegmentCode)
            .NotEmpty()
            .WithMessage("SegmentCode è obbligatorio");

        RuleFor(x => x.KmType)
            .NotEmpty()
            .WithMessage("KmType è obbligatorio");
    }
}