using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Application.Estimate.Helpers;

namespace Itera.BookingService.Infrastructure.Estimate;

/// <summary>
/// Implementazione di <see cref="IDurationService"/>.
/// Replica DurataBL.CalcolaDurata24HByDate (H24, tolleranza 60 min hard-coded).
/// Non dipende da DB: le soglie plurimensile/weekend sono costanti legacy.
/// </summary>
internal sealed class DurationService : IDurationService
{
    // Soglia plurimensile: default 31 giorni (GiorniSogliaPlurimensile legacy).
    // Nessun DB di configurazione esposto nell'infrastruttura WS; valore fisso come nel legacy.
    private const int SogliaPlurimensile = 31;

    // Tolleranza oraria: default 60 min (TolleranzaMinGiorno legacy).
    // Nel contesto WS GetKms non arriva né un listinoID né un contrattoID,
    // quindi si usa il fallback hard-coded, identico al legacy.
    private const int TolleranzaMinuti = 60;

    public DurationResult Calcola(DateTime dataFrom, DateTime dataTo, bool venditaGiornoSingoloSuWeekend = true)
    {
        TimeSpan diff = dataTo - dataFrom;
        int giorni = diff.Days;

        // Aggiunge un giorno se i minuti residui superano la tolleranza
        double minutiResiduiDelGiorno = diff.TotalMinutes % 1440;
        if (minutiResiduiDelGiorno > TolleranzaMinuti)
            giorni++;

        if (giorni <= 0)
            giorni = 1;

        string codiceDurata;
        DateTime? newDataTo = null;

        if (giorni <= 29)
        {
            // Verifica weekend standard (venerdì-domenica o sabato/domenica -> lunedì)
            bool isWeekend = IsWeekendStandard(giorni, dataFrom, dataTo, venditaGiornoSingoloSuWeekend);
            if (isWeekend)
            {
                codiceDurata = DurationHelper.CodiceWeekend;
                giorni = 2; // weekend conta sempre 2 giorni nella tariffa
            }
            else
            {
                codiceDurata = DurationHelper.CodiceGiorno;
            }
        }
        else if (giorni < SogliaPlurimensile)
        {
            codiceDurata = DurationHelper.CodiceMese;
            // newDataTo non viene impostato qui: nel contesto GetKms non serve
        }
        else
        {
            codiceDurata = DurationHelper.CodicePlurimensile;
            newDataTo = dataFrom.AddDays(giorni);
        }

        return new DurationResult(codiceDurata, giorni, newDataTo);
    }

    /// <summary>
    /// Logica weekend standard (senza listino dinamico): venerdì/sabato/domenica/lunedì.
    /// Replica IsWeekend(ref giorni, ...) del legacy con listino == null.
    /// </summary>
    private static bool IsWeekendStandard(int giorni, DateTime dataFrom, DateTime dataTo, bool venditaGiornoSingoloSuWeekend)
    {
        // Giorno singolo su weekend: se venditaGiornoSingoloSuWeekend == true viene escluso
        if (giorni == 1 &&
            (dataFrom.DayOfWeek == DayOfWeek.Saturday || dataFrom.DayOfWeek == DayOfWeek.Sunday) &&
            venditaGiornoSingoloSuWeekend)
            return false;

        var giorniWeekend = new[]
        {
            DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday
        };

        if (!giorniWeekend.Contains(dataFrom.DayOfWeek) || !giorniWeekend.Contains(dataTo.DayOfWeek))
            return false;

        if (giorni < 1 || giorni > 3)
            return false;

        // Venerdì: ritiro da ora 17 in poi (OraFineDefault = 17 nel legacy)
        if (dataFrom.DayOfWeek == DayOfWeek.Friday && dataFrom.Hour < 17)
            return false;

        // Lunedì: consegna fino alle 9 (OraInizioDefault = 9 nel legacy)
        if (dataTo.DayOfWeek == DayOfWeek.Monday && dataTo.Hour > 9)
            return false;

        return true;
    }
}
