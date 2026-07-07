namespace Itera.BookingService.Infrastructure.Estimate.Mapping;

public sealed class PeriodoCompetenza
{
    public int GiorniPeriodo { get; init; }
    public bool RateoGiorniExtra { get; init; }

    public bool IsRateoGiorniExtra() => RateoGiorniExtra;

    public static PeriodoCompetenza None() => new()
    {
        GiorniPeriodo = 0,
        RateoGiorniExtra = false
    };
}