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
    public async Task<List<WsKmOpzione>> GetKmsAsync(
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

        // TODO #6: sostituire il mapping inline con mapper.Map<WsKmOpzione>(km)
        //          una volta completata EstimateMappingConfig (Issue #6).
        var result = await db.ListinoKm
            .AsNoTracking()
            .Where(km =>
                km.FilialeId  == filialeId  &&
                km.CategoriaId == categoriaId &&
                (km.FasciaOrarioRitiro  == null || km.FasciaOrarioRitiro  == fasciaOrarioRitiro)  &&
                (km.FasciaOrarioConsegna == null || km.FasciaOrarioConsegna == fasciaOrarioConsegna))
            .OrderBy(km => km.KmId)
            .Select(km => new WsKmOpzione
            {
                KmId       = km.KmId,
                Descrizione = km.Descrizione,
                Selected   = km.Selected
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}
