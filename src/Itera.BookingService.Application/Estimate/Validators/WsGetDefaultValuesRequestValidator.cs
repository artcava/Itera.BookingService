using FluentValidation;

public sealed class WsGetDefaultValuesRequestValidator
    : AbstractValidator<GetDefaultValuesRequest>
{
    public WsGetDefaultValuesRequestValidator()
    {
        // BrandID è ereditato da WsRequest e sempre obbligatorio
        RuleFor(x => x.BrandID)
            .GreaterThan((short)0)
            .WithMessage("BrandID non valido");

        // BranchID opzionale, ma se presente deve essere > 0
        RuleFor(x => x.BranchID)
            .GreaterThan(0)
            .When(x => x.BranchID.HasValue)
            .WithMessage("BranchID non valido");

        // DebugDateToday: nessuna validazione strutturale nel validator
        // (nessuna dipendenza esterna, parsing avviene nel service)
    }
}