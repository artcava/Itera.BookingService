using Itera.BookingService.Contracts.Estimate;

namespace Itera.BookingService.Application.Abstractions;

/// <summary>
/// Contratto Infrastructure per la lettura delle opzioni km disponibili
/// in funzione di filiale, categoria veicolo e finestra temporale.
/// </summary>
public interface IKmQueryService
{
    Task<List<KmOpzione>> GetKmsAsync(
        int    filialeId,
        string categoriaId,
        DateTime dataFrom,
        DateTime dataTo,
        int    fasciaOrarioRitiro,
        int    fasciaOrarioConsegna,
        CancellationToken cancellationToken);
}
