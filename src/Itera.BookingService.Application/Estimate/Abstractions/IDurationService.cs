namespace Itera.BookingService.Application.Estimate.Abstractions;

/// <summary>
/// Calcola durata e giorni a partire da dataFrom/dataTo,
/// replicando DurataBL.CalcolaDurata24HByDate del legacy.
/// </summary>
public interface IDurationService
{
    /// <summary>
    /// Calcola la durata del noleggio con logica H24 + tolleranza oraria.
    /// </summary>
    /// <param name="dataFrom">Data/ora inizio noleggio.</param>
    /// <param name="dataTo">Data/ora fine noleggio.</param>
    /// <param name="venditaGiornoSingoloSuWeekend">
    ///     Se true, un giorno singolo in weekend è classificato come weekend.
    /// </param>
    /// <returns>Un <see cref="DurationResult"/> con CodiceDurata, Giorni e NewDataTo.</returns>
    DurationResult Calcola(DateTime dataFrom, DateTime dataTo, bool venditaGiornoSingoloSuWeekend = true);
}

/// <summary>
/// Risultato del calcolo durata.
/// </summary>
/// <param name="CodiceDurata">"G", "W", "M" o "P".</param>
/// <param name="Giorni">Numero di giorni calcolati (min 1).</param>
/// <param name="NewDataTo">
///     Valorizzato solo per Mese/Plurimensile quando il periodo eccede la soglia.
///     Rappresenta la data di fine "normalizzata" al mese.
/// </param>
public record DurationResult(
    string CodiceDurata,
    int Giorni,
    DateTime? NewDataTo = null
);
