using FluentValidation;
using Itera.BookingService.Contracts.Legacy.Estimate;

namespace Itera.BookingService.Application.Estimate;

public sealed class WsGetProvinceRequestValidator : AbstractValidator<GetProvinceRequest>
{
    public WsGetProvinceRequestValidator()
    {
        // Nessun campo obbligatorio: compatibilità legacy.
    }
}
