using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Application.Helpers;

namespace Itera.BookingService.Application.Estimate;

/// <summary>
/// Implementazione di <see cref="IDurationService"/>.
/// Replica la logica di DurataBL.CalcolaDurata24HByDate del legacy.
/// </summary>
public sealed class DurationService : IDurationService
{
    public DurationResult Calcola(DateTime dataFrom, DateTime dataTo, bool venditaGiornoSingoloSuWeekend = true)
    {
        // Calcolo giorni con logica H24 + tolleranza: ogni 24h complete = 1 giorno,
        // se il residuo supera TolleranzaOre si aggiunge un giorno extra.
        var diff    = dataTo - dataFrom;
        var giorni  = (int)diff.TotalHours / DurationHelper.HoursInDay;
        var residuo = (int)diff.TotalHours % DurationHelper.HoursInDay;

        if (residuo > DurationHelper.TolleranzaOre)
            giorni++;

        giorni = Math.Max(1, giorni);

        // Determinazione codice durata
        if (giorni >= DurationHelper.SogliaMese)
        {
            // Tronca la finestra a esattamente un mese (stesso giorno del mese successivo).
            // NewDataTo segnala al chiamante il "PeriodoSuperioreAlMese".
            var newDataTo = dataFrom.AddMonths(1);
            var giorniMese = (int)(newDataTo - dataFrom).TotalDays;
            return new DurationResult(DurationHelper.CodicePlurimensile, giorniMese, newDataTo);
        }

        if (giorni >= DurationHelper.SogliaMese - 1) // 27 giorni = un mese intero
            return new DurationResult(DurationHelper.CodiceMese, giorni);

        // Weekend: sabato + domenica (2gg) o venerdì + sabato + domenica (3gg)
        var isWeekend = IsWeekend(dataFrom, giorni, venditaGiornoSingoloSuWeekend);
        if (isWeekend)
        {
            var codiceW = giorni >= DurationHelper.GiorniWeekend3g
                ? DurationHelper.CodiceWeekend
                : DurationHelper.CodiceWeekend;
            return new DurationResult(codiceW, giorni);
        }

        return new DurationResult(DurationHelper.CodiceGiorno, giorni);
    }

    private static bool IsWeekend(DateTime dataFrom, int giorni, bool venditaGiornoSingoloSuWeekend)
    {
        if (giorni > DurationHelper.GiorniWeekend3g) return false;

        var dow = dataFrom.DayOfWeek;

        // Noleggio che inizia venerdì, sabato o domenica con max 3 giorni
        if (dow is DayOfWeek.Friday or DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            if (giorni == 1 && venditaGiornoSingoloSuWeekend) return true;
            if (giorni is DurationHelper.GiorniWeekend or DurationHelper.GiorniWeekend3g)   return true;
        }

        return false;
    }
}
