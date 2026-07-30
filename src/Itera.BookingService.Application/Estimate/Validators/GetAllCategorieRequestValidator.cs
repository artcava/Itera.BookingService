using FluentValidation;
using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Validators;

public sealed class GetAllCategorieRequestValidator : AbstractValidator<GetAllCategorieRequest>
{
    public GetAllCategorieRequestValidator()
    {
        // Nessun campo obbligatorio: compatibilità legacy.
    }
}
