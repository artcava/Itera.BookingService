using Itera.BookingService.Contracts.Abstractions;

namespace Itera.BookingService.Contracts.Estimate;

public class GetAllCategorieRequest : ITokenCarrier
{
    public string? Token    { get; set; }
    public string? Language { get; set; }
}
