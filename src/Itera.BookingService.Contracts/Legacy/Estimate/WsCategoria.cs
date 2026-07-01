namespace Itera.BookingService.Contracts.Legacy.Estimate;

public class WsCategoria
{
    public const string Auto     = "A";
    public const string Furgone  = "F";
    public const string Frigo    = "I";
    public const string Mobility = "M";
    public const string FurgoneFrigo = "FI";

    public string CategoryID    { get; set; } = default!;
    public string Description   { get; set; } = default!;
}
