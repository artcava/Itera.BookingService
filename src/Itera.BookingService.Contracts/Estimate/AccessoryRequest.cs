namespace Itera.BookingService.Contracts.Estimate;

public sealed record AccessoryRequest
{
    public short AccessoryID { get; init; }
    public short Quantity { get; init; }
}
