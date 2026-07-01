using System.Text.Json.Serialization;

public sealed class WsGetDefaultValues
{
    [JsonPropertyName("DateFromFormatted")]
    public string DateFromFormatted { get; set; }

    [JsonPropertyName("DateToFormatted")]
    public string DateToFormatted { get; set; }

    public int KmID { get; set; }

    [JsonPropertyName("CategoryID")]
    public string CategoryID { get; set; }
}