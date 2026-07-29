using System.Text.Json.Serialization;

namespace Itera.BookingService.Contracts.Estimate;

public sealed record InsuranceExtraDto
{
    public int InsuranceExtraID { get; init; }
    public string? InsuranceExtraDescr { get; init; }
    public string? InsuranceExtra { get; init; }
    public string? InsuranceExtraWithoutIVA { get; init; }
    public string? Type { get; set; }
    public string? CategoryID { get; set; }
}

public sealed record AmountEstimateDto
{
    public AmountEstimateValueDto Amounts { get; init; } = new();
    public AmountEstimateValueDto AmountsWithoutDiscount { get; init; } = new();
    public List<AmountEstimateDiscountDto>? Discount { get; init; }
}

public sealed record AmountEstimateValueDto
{
    public string Amount { get; init; } = string.Empty;
    public string AmountWithoutIVA { get; init; } = string.Empty;
}

public sealed record AmountEstimateDiscountDto
{
    [JsonPropertyName("HDN_SCN")]
    public decimal? HDN_SCN { get; init; }

    [JsonPropertyName("HDN_SCN_KEY")]
    public string? HDN_SCN_KEY { get; init; }

    public short DiscountTypeID { get; init; }
    public string? DiscountTypeDescription { get; init; }
    public int? RegolaDiVenditaID { get; init; }

    [JsonPropertyName("DiscountIdError")]
    public int? DiscountIdError { get; init; }

    [JsonPropertyName("DiscountMessageError")]
    public string? DiscountMessageError { get; init; }
}