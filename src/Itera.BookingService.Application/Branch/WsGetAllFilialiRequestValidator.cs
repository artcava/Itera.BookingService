using FluentValidation;
using Itera.BookingService.Contracts.Legacy.Branch;

namespace Itera.BookingService.Application.Branch;

public class WsGetAllFilialiRequestValidator : AbstractValidator<WsGetAllFilialiRequest>
{
    public WsGetAllFilialiRequestValidator()
    {
        // Intentionally permissive for legacy parity.
    }
}
