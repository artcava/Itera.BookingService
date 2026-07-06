using FluentValidation;
using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Validators;

public sealed class GetNationsRequestValidator : AbstractValidator<GetNationsRequest>
{
    public GetNationsRequestValidator()
    {
        // Nessun campo obbligatorio oltre al token (già validato dal filter)
        // Language è opzionale; se assente si usa ITA come default
    }
}