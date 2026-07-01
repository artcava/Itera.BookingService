using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy.Estimate;
using Itera.BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Infrastructure.Estimate;

public sealed class LegacyKmQueryService(
    LegacyDbContext db,
    ILogger<LegacyKmQueryService> logger) : IKmQueryService
{
    public async Task<List<KmOpzione>> GetKmsAsync(
        int      filialeId,
        string   categoriaId,
        DateTime dataFrom,
        DateTime dataTo,
        int      fasciaOrarioRitiro,
        int      fasciaOrarioConsegna,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "GetKms: filialeId={FilialeId} categoriaId={CategoriaId} dataFrom={DataFrom} dataTo={DataTo}",
            filialeId, categoriaId, dataFrom, dataTo);

        // Risoluzione listino attivo per la filiale nella finestra temporale richiesta.
        // Un listino è valido se SempreAttivo=true OPPURE la finestra [dataFrom,dataTo]
        // cade dentro [InizioValidita, FineValidita].
        // La fascia oraria è verificata come range: FasciaStart <= fascia <= FasciaEnd
        // (sia ritiro che consegna devono ricadere nello stesso ListinoGiorni).
        //
        // TODO #6: sostituire il mapping inline con mapper.Map<WsKmOpzione>()
        //          una volta completata EstimateMappingConfig (Issue #6).

        var result = await (
            from lf  in db.ListinoFiliali
            join l   in db.Listini        on lf.ListinoId       equals l.ListinoId
            join lg  in db.ListinoGiorni  on l.ListinoId        equals lg.ListinoId
            join lkm in db.ListinoKm      on lg.ListinoGiorniId equals lkm.ListinoGiorniId
            where
                lf.FilialeId == filialeId &&
                lg.CodiceCategoria == categoriaId &&
                (lg.IsVisible == null || lg.IsVisible == true) &&
                (lkm.IsVisible == null || lkm.IsVisible == true) &&
                (l.SempreAttivo ||
                    (l.InizioValidita <= dataFrom && (l.FineValidita == null || l.FineValidita >= dataTo))) &&
                lg.FasciaStart <= fasciaOrarioRitiro   && fasciaOrarioRitiro   <= lg.FasciaEnd &&
                lg.FasciaStart <= fasciaOrarioConsegna && fasciaOrarioConsegna <= lg.FasciaEnd
            orderby lkm.Ordinamento
            select new KmOpzione
            {
                // KmId: usiamo il valore int come stringa, coerente con WsKm.KmID legacy.
                // Km=0 convenzionalmente = illimitati (verificare con dati reali).
                KmId        = lkm.Km.ToString(),
                Descrizione = lkm.Km == 0 ? "Illimitati" : $"{lkm.Km} km",
                Selected    = lkm.Ordinamento == (
                    db.ListinoKm
                        .Where(x => x.ListinoGiorniId == lkm.ListinoGiorniId)
                        .Min(x => x.Ordinamento))
            })
            .AsNoTracking()
            .Distinct()
            .ToListAsync(cancellationToken);

        return result;
    }
}
