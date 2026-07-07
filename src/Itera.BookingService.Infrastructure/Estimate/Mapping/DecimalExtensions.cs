namespace Itera.BookingService.Infrastructure.Estimate.Mapping;

public static class DecimalExtensions
{
    public static decimal Floor(this decimal value) => Math.Floor(value);
}