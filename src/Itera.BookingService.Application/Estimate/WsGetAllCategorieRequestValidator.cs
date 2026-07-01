using FluentValidation;
using Itera.BookingService.Contracts.Legacy.Estimate;

namespace Itera.BookingService.Application.Estimate;

public sealed class WsGetAllCategorieRequestValidator : AbstractValidator<WsGetAllCategorieRequest>
{
    public WsGetAllCategorieRequestValidator()
    {
        // Nessun campo obbligatorio: compatibilità legacy.
    }
}
