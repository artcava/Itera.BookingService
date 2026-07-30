using System.Text.Json.Serialization;

namespace Itera.BookingService.Contracts.Estimate;

public sealed record EstimateDto
{
    [JsonPropertyName("Segments")]
    public List<EstimateSegmentDto> Segments { get; init; } = [];

    [JsonPropertyName("RentalDays")]
    public string? RentalDays { get; init; }

    [JsonPropertyName("Catalog")]
    public string? Catalog { get; init; }

    [JsonPropertyName("PeriodID")]
    public string? PeriodId { get; init; }

    [JsonPropertyName("PeriodOpenExtra")]
    public bool PeriodOpenExtra { get; init; }

    [JsonPropertyName("TokenEstimate")]
    public Guid TokenEstimate { get; init; }

    [JsonPropertyName("TimeOutSecondsToken")]
    public int TimeOutSecondsToken { get; init; }

    [JsonPropertyName("Val")]
    public bool Val { get; init; }

    [JsonPropertyName("CommercialAgreementCode")]
    public string? CommercialAgreementCode { get; init; }

    [JsonPropertyName("CommercialAgreementDescription")]
    public string? CommercialAgreementDescription { get; init; }

    [JsonPropertyName("Details")]
    public EstimateDetailsDto? Details { get; init; }

    [JsonPropertyName("AdditionalData")]
    public EstimateAdditionalDataDto? AdditionalData { get; init; }

    [JsonPropertyName("PrePayment")]
    public bool? PrePayment { get; init; }
}

public sealed record EstimateSegmentDto
{
    [JsonPropertyName("AmountSegmentEstimate")]
    public List<EstimateSegmentAmountDto> AmountSegmentEstimate { get; init; } = [];

    [JsonPropertyName("CatalogID")]
    public int CatalogId { get; init; }

    [JsonPropertyName("DiscountList")]
    public List<AmountEstimateDiscountDto>? DiscountList { get; init; }

    [JsonPropertyName("StateClusterBranch")]
    public int StateClusterBranch { get; init; }

    [JsonPropertyName("StateClusterSegment")]
    public string? StateClusterSegment { get; init; }

    [JsonPropertyName("SegmentUpgrade")]
    public EstimateSegmentUpgradeDto? SegmentUpgrade { get; init; }
}

public sealed record EstimateSegmentAmountDto
{
    [JsonPropertyName("AmountRental")]
    public string? AmountRental { get; init; }

    [JsonPropertyName("AmountPrepaid")]
    public string? AmountPrepaid { get; init; }

    [JsonPropertyName("AmountToBePaid")]
    public string? AmountToBePaid { get; init; }

    [JsonPropertyName("KmType")]
    public string? KmType { get; init; }

    [JsonPropertyName("KmIncluded")]
    public string? KmIncluded { get; init; }

    [JsonPropertyName("Amount")]
    public string? Amount { get; init; }

    [JsonPropertyName("AmountWithoutIVA")]
    public string? AmountWithoutIVA { get; init; }

    [JsonPropertyName("AmountWithoutInsurance")]
    public string? AmountWithoutInsurance { get; init; }

    [JsonPropertyName("AmountWithoutInsurance&IVA")]
    public string? AmountWithoutInsuranceAndIVA { get; init; }

    [JsonPropertyName("KmExtra")]
    public string? KmExtra { get; init; }

    [JsonPropertyName("KmExtraWithoutIVA")]
    public string? KmExtraWithoutIVA { get; init; }

    [JsonPropertyName("Val")]
    public string? Val { get; init; }

    [JsonPropertyName("ValWithoutIVA")]
    public string? ValWithoutIVA { get; init; }

    [JsonPropertyName("AmountsWithoutDiscount")]
    public EstimateAmountValueDto? AmountsWithoutDiscount { get; init; }

    [JsonPropertyName("AccessoriesIncluded")]
    public List<EstimateAccessoryDto>? AccessoriesIncluded { get; init; }

    [JsonPropertyName("InsurancesIncluded")]
    public List<EstimateInsuranceDto>? InsurancesIncluded { get; init; }

    [JsonPropertyName("AccessoriesBought")]
    public List<EstimateAccessoryAmountDto>? AccessoriesBought { get; init; }

    [JsonPropertyName("InsurancesBought")]
    public List<EstimateInsuranceAmountDto>? InsurancesBought { get; init; }

    [JsonPropertyName("RegolaDiVenditaID")]
    public string? RegolaDiVenditaID { get; init; }

    [JsonPropertyName("DiscountList")]
    public List<AmountEstimateDiscountDto>? DiscountList { get; init; }

    [JsonPropertyName("EnablePrepaid")]
    public bool EnablePrepaid { get; init; }
}

public sealed record EstimateAmountValueDto
{
    [JsonPropertyName("Amount")]
    public string? Amount { get; init; }

    [JsonPropertyName("AmountWithoutIVA")]
    public string? AmountWithoutIVA { get; init; }

    [JsonPropertyName("AmountWithoutInsurance")]
    public string? AmountWithoutInsurance { get; init; }

    [JsonPropertyName("AmountWithoutInsurance&IVA")]
    public string? AmountWithoutInsuranceAndIVA { get; init; }
}

public sealed record EstimateAccessoryDto
{
    [JsonPropertyName("AccessoryID")]
    public short AccessoryID { get; init; }

    [JsonPropertyName("Description")]
    public string? Description { get; init; }
}

public sealed record EstimateInsuranceDto
{
    [JsonPropertyName("InsuranceExtraID")]
    public int InsuranceExtraID { get; init; }

    [JsonPropertyName("InsuranceExtra")]
    public string? InsuranceExtra { get; init; }
}

public sealed record EstimateAccessoryAmountDto
{
    [JsonPropertyName("AccessoryID")]
    public short AccessoryID { get; init; }

    [JsonPropertyName("Quantity")]
    public int Quantity { get; init; }

    [JsonPropertyName("Amount")]
    public string? Amount { get; init; }
}

public sealed record EstimateInsuranceAmountDto
{
    [JsonPropertyName("InsuranceExtraID")]
    public int InsuranceExtraID { get; init; }

    [JsonPropertyName("Type")]
    public string? Type { get; init; }

    [JsonPropertyName("Amount")]
    public string? Amount { get; init; }
}

public sealed record EstimateSegmentUpgradeDto
{
    [JsonPropertyName("CodeSegment")]
    public string? CodeSegment { get; init; }

    [JsonPropertyName("Description")]
    public string? Description { get; init; }
}

public sealed record EstimateDetailsDto
{
    [JsonPropertyName("BranchID")]
    public int BranchID { get; init; }

    [JsonPropertyName("BranchDestinationID")]
    public int BranchDestinationID { get; init; }

    [JsonPropertyName("DateFrom")]
    public string? DateFrom { get; init; }

    [JsonPropertyName("DateTo")]
    public string? DateTo { get; init; }
}

public sealed record EstimateAdditionalDataDto
{
    [JsonPropertyName("Notes")]
    public string? Notes { get; init; }

    [JsonPropertyName("DossierNumber")]
    public string? DossierNumber { get; init; }

    [JsonPropertyName("ContactEmail")]
    public string? ContactEmail { get; init; }

    [JsonPropertyName("TrainFlyNumber")]
    public string? TrainFlyNumber { get; init; }

    [JsonPropertyName("VchCard")]
    public string? VchCard { get; init; }

    [JsonPropertyName("Privacy")]
    public bool Privacy { get; init; }

    [JsonPropertyName("NewsletterSubscription")]
    public bool NewsletterSubscription { get; init; }
}