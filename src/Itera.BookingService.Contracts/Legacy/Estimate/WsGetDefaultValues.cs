using System.Text.Json.Serialization;

namespace Itera.BookingService.Contracts.Legacy.Estimate;

/// <summary>
/// DTO risposta per GetDefaultValues.
/// KmID non è esposto: la BL legacy WsPreventivoBL.GetDefaultValues
/// non valorizza mai il campo km — non fa parte del contratto di risposta.
/// </summary>
public sealed class WsGetDefaultValues
{
    [JsonPropertyName("DateFromFormatted")]
    public string DateFromFormatted { get; set; } = string.Empty;

    [JsonPropertyName("DateToFormatted")]
    public string DateToFormatted { get; set; } = string.Empty;

    [JsonPropertyName("CategoryID")]
    public string CategoryID { get; set; } = string.Empty;
}