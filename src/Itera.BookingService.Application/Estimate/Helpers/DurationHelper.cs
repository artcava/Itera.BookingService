namespace Itera.BookingService.Application.Estimate.Helpers;

/// <summary>
/// Funzioni pure per il calcolo della durata.
/// Replica la logica di KmHelper.CalcolaCodiceGiorno e DurataHelper dal legacy.
/// Non ha dipendenze esterne: testabile in isolamento.
/// </summary>
public static class DurationHelper
{
    // Codici durata — replica di DurataHelper._CODICEDURATA_* nel legacy
    public const string CodiceGiorno       = "G";
    public const string CodiceWeekend      = "W";
    public const string CodiceMese         = "M";
    public const string CodicePlurimensile = "P";

    // Valori CodiceGiorno sul DB (colonna ListinoGiorni.Codice)
    public const string DbCodiceWeekend    = "Weekend";
    public const string DbCodiceWeekend3g  = "Weekend3g";
    public const string DbCodiceMese       = "Mese";
    public const string DbCodicePlurimensile = "Plurimensile";
    public const string DbCodiceGiorni2    = "Giorni2";

    /// <summary>
    /// Calcola il codice colonna <c>ListinoGiorni.Codice</c> a partire dal codice durata e
    /// dal numero di giorni del noleggio.
    /// Replica fedele di <c>KmHelper.CalcolaCodiceGiorno</c> nel legacy.
    /// </summary>
    public static string CalcolaCodiceGiorno(string codiceDurata, int giorni) =>
        codiceDurata switch
        {
            CodiceGiorno       => $"Giorni{giorni}",
            CodiceWeekend      => giorni == 3 ? DbCodiceWeekend3g : DbCodiceWeekend,
            CodiceMese         => DbCodiceMese,
            CodicePlurimensile => DbCodicePlurimensile,
            _                  => DbCodiceGiorni2
        };

    /// <summary>
    /// Restituisce true se il codice durata è mensile o plurimensile.
    /// Usato dal filtro "illimitati" in GetKms.
    /// </summary>
    public static bool IsMensileOPlurimensile(string codiceDurata) =>
        codiceDurata == CodiceMese || codiceDurata == CodicePlurimensile;
}
