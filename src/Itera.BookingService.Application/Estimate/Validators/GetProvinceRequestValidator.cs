using FluentValidation;

namespace Itera.BookingService.Application.Estimate.Validators;
public sealed class GetProvinceRequestValidator : AbstractValidator<GetProvinceRequest>
{
    public GetProvinceRequestValidator()
    {
        // Nessuna regola di business: la request non trasporta parametri
        // La validazione del token è gestita da LegacyTokenEndpointFilter
    }
}