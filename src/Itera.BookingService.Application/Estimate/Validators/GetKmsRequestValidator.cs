using FluentValidation;
using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Estimate.Validators;

/// <summary>
/// Validatore per <see cref="WsGetKmsRequest"/>.
/// Nessuna dipendenza esterna: solo regole sintattiche/semantiche sui campi.
/// </summary>
public sealed class GetKmsRequestValidator : AbstractValidator<GetKmsRequest>
{
    private const string DateFormat = "yyyy-MM-ddTHH:mm:ss";

    public GetKmsRequestValidator()
    {
        RuleFor(x => x.FilialeId)
            .GreaterThan(0)
            .WithMessage("FilialeId deve essere maggiore di zero.");

        RuleFor(x => x.CategoriaId)
            .NotEmpty()
            .WithMessage("CategoriaId è obbligatoria.");

        RuleFor(x => x.DataFrom)
            .NotEmpty()
            .WithMessage("DataFrom è obbligatoria.")
            .Must(BeValidDate)
            .WithMessage($"DataFrom deve essere nel formato {DateFormat}.");

        RuleFor(x => x.DataTo)
            .NotEmpty()
            .WithMessage("DataTo è obbligatoria.")
            .Must(BeValidDate)
            .WithMessage($"DataTo deve essere nel formato {DateFormat}.");

        RuleFor(x => x)
            .Must(r => ParseDate(r.DataFrom) < ParseDate(r.DataTo))
            .When(r => BeValidDate(r.DataFrom) && BeValidDate(r.DataTo))
            .WithName("DataTo")
            .WithMessage("DataTo deve essere successiva a DataFrom.");

        RuleFor(x => x.FasciaOrarioRitiro)
            .GreaterThan(0)
            .WithMessage("FasciaOrarioRitiro deve essere maggiore di zero.");

        RuleFor(x => x.FasciaOrarioConsegna)
            .GreaterThan(0)
            .WithMessage("FasciaOrarioConsegna deve essere maggiore di zero.");
    }

    private static bool BeValidDate(string? value) =>
        DateTime.TryParseExact(value, DateFormat,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out _);

    private static DateTime ParseDate(string? value) =>
        DateTime.ParseExact(value!, DateFormat,
            System.Globalization.CultureInfo.InvariantCulture);
}
