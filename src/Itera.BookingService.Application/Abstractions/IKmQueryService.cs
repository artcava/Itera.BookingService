using Itera.BookingService.Contracts.Legacy.Estimate;

namespace Itera.BookingService.Application.Abstractions;

/// <summary>
/// Query service per il recupero delle opzioni chilometriche disponibili
/// per un listino, una durata e un numero di giorni specifici.
/// </summary>
public interface IKmQueryService
{
    /// <summary>
    /// Restituisce le opzioni km disponibili per il listino e la durata indicati.
    /// </summary>
    /// <param name="listinoId">Identificativo del listino.</param>
    /// <param name="codiceDurata">Codice durata: "G", "W", "M" o "P".</param>
    /// <param name="giorni">Numero di giorni calcolati dal <see cref="IDurationService"/>.</param>
    /// <param name="cancellationToken">Token di cancellazione.</param>
    Task<List<WsKmOpzione>> GetKmsAsync(
        int listinoId,
        string codiceDurata,
        int giorni,
        CancellationToken cancellationToken);
}
