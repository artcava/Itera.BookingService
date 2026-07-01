using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Application.Estimate.Helpers;

namespace Itera.BookingService.Application.Estimate;

/// <summary>
/// Implementazione di <see cref="IDurationService"/>.
/// Replica la logica di DurataBL.CalcolaDurata24HByDate del legacy.
/// </summary>
public sealed class DurationService : IDurationService
{
    // Soglia in ore oltre la quale si applica la tolleranza H24
    private const int TolleranzaOre = 2;

    // Soglia giorni per weekend singolo vs weekend esteso (3 giorni)
    private const int GiorniWeekend     = 2;
    private const int GiorniWeekend3g   = 3;

    // Soglia giorni per passare a mensile (28 giorni = 4 settimane)
    private const int SogliaMese        = 28;

    public DurationResult Calcola(DateTime dataFrom, DateTime dataTo, bool venditaGiornoSingoloSuWeekend = true)
    {
        // Calcolo giorni con logica H24 + tolleranza: ogni 24h complete = 1 giorno,
        // se il residuo supera TolleranzaOre si aggiunge un giorno extra.
        var diff    = dataTo - dataFrom;
        var giorni  = (int)diff.TotalHours / 24;
        var residuo = (int)diff.TotalHours % 24;

        if (residuo > TolleranzaOre)
            giorni++;

        giorni = Math.Max(1, giorni);

        // Determinazione codice durata
        if (giorni >= SogliaMese)
        {
            // Tronca la finestra a esattamente un mese (stesso giorno del mese successivo).
            // NewDataTo segnala al chiamante il "PeriodoSuperioreAlMese".
            var newDataTo = dataFrom.AddMonths(1);
            var giorniMese = (int)(newDataTo - dataFrom).TotalDays;
            return new DurationResult(DurationHelper.CodicePlurimensile, giorniMese, newDataTo);
        }

        if (giorni >= SogliaMese - 1) // 27 giorni = un mese intero
            return new DurationResult(DurationHelper.CodiceMese, giorni);

        // Weekend: sabato + domenica (2gg) o venerdì + sabato + domenica (3gg)
        var isWeekend = IsWeekend(dataFrom, dataTo, giorni, venditaGiornoSingoloSuWeekend);
        if (isWeekend)
        {
            var codiceW = giorni >= GiorniWeekend3g
                ? DurationHelper.CodiceWeekend
                : DurationHelper.CodiceWeekend;
            return new DurationResult(codiceW, giorni);
        }

        return new DurationResult(DurationHelper.CodiceGiorno, giorni);
    }

    private static bool IsWeekend(DateTime dataFrom, DateTime dataTo, int giorni, bool venditaGiornoSingoloSuWeekend)
    {
        if (giorni > GiorniWeekend3g) return false;

        var dow = dataFrom.DayOfWeek;

        // Noleggio che inizia venerdì, sabato o domenica con max 3 giorni
        if (dow is DayOfWeek.Friday or DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            if (giorni == 1 && venditaGiornoSingoloSuWeekend) return true;
            if (giorni is GiorniWeekend or GiorniWeekend3g)   return true;
        }

        return false;
    }
}
