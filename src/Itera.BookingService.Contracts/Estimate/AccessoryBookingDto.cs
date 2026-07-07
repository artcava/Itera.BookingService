namespace Itera.BookingService.Contracts.Estimate;

public sealed record AccessoryBookingDto
{
    public short AccessoryId { get; init; }
    public byte InvoiceLineTypeId { get; init; }
    public string? Code { get; init; }
    public string? Description { get; init; }
    public short? CategoryId { get; init; }
    public string? CategoryCode { get; init; }
    public string? CategoryDescription { get; init; }
    public bool Mandatory { get; init; }
    public bool Preselected { get; init; }
    public bool PrepaidWeb { get; init; }
    public decimal Amount { get; init; }
    public decimal AmountVat { get; init; }
    public short? VatId { get; init; }
    public string? MomentOfSale { get; init; }
}