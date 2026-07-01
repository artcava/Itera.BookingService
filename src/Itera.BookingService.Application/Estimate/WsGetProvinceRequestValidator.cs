using FluentValidation;
using Itera.BookingService.Contracts.Legacy.Estimate;

namespace Itera.BookingService.Application.Estimate;

public sealed class GetProvinceRequestValidator : AbstractValidator<GetProvinceRequest>
{
    public GetProvinceRequestValidator()
    {
        // Nessun campo obbligatorio: compatibilità legacy.
    }
}
