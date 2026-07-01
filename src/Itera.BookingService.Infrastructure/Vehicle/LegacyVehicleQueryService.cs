using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Vehicle;
using Itera.BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Itera.BookingService.Infrastructure.Vehicle;

public sealed class LegacyVehicleQueryService(LegacyDbContext db) : IVehicleQueryService
{
    public async Task<List<MezzoSegmento>> GetMezziAsync(
        string? fleetMulti,
        string? segmentoMulti,
        bool? mezzoSpeciale,
        int? gruppoId,
        CancellationToken cancellationToken)
    {
        var fleetList    = fleetMulti?   .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var segmentoList = segmentoMulti?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        // Carichiamo l'insieme dei segmenti speciali solo quando il filtro è attivo.
        // SegmentoModelloMezzoSpeciale è una piccola tabella di lookup: il round-trip
        // separato è preferibile a una subquery inline non traducibile da EF Core.
        HashSet<string>? specialSet = null;
        if (mezzoSpeciale.HasValue)
        {
            var specialCodes = await db.SegmentiMezzoSpeciali
                .Select(s => s.CodiceSegmento)
                .ToListAsync(cancellationToken);
            specialSet = new HashSet<string>(specialCodes, StringComparer.OrdinalIgnoreCase);
        }

        var query =
            from mm  in db.ModelliMezzo
            join sm  in db.SegmentiModello       on mm.CodiceSegmento equals sm.CodiceSegmento
            join am  in db.AlimentazioniModello  on mm.AlimentazioneModelloID equals am.AlimentazioneModelloID
            join ma  in db.Marche                on mm.MarcaID equals ma.MarcaID
            join smc in db.SegmentiModelloClasse on sm.SegmentoModelloClasseID equals smc.SegmentoModelloClasseID
            join mmg in db.ModelliMezzoGruppo    on mm.ModelloMezzoID equals mmg.ModelloMezzoID into mmgLeft
            from mmg in mmgLeft.DefaultIfEmpty()
            where (mm.VisibilitaSito == null || mm.VisibilitaSito == true)
               && (fleetList    == null || fleetList.Contains(sm.FleetID))
               && (segmentoList == null || segmentoList.Contains(sm.CodiceSegmento))
               && (gruppoId     == null || (mmg != null && mmg.GruppoID == gruppoId))
            orderby sm.Ordinamento ascending
            select new MezzoSegmento
            {
                ModelloMezzoID                     = mm.ModelloMezzoID,
                Marca                              = ma.Descrizione,
                ModelloDescr                       = mm.Descrizione,
                NomeImmagine                       = mm.NomeImmagine,
                Cilindrata                         = mm.Cilindrata,
                AlimentazioneModelloID             = am.AlimentazioneModelloID,
                AlimentazioneDescr                 = am.Descrizione,
                Euro                               = mm.Euro,
                NumeroPosti                        = mm.NumeroPosti,
                NumeroPorte                        = mm.NumeroPorte,
                Autoradio                          = mm.Autoradio,
                AriaCondizionata                   = mm.AriaCondizionata,
                Abs                                = mm.Abs,
                Airbag                             = mm.Airbag,
                CapacitaSerbatoio                  = mm.CapacitaSerbatoio,
                Portata                            = mm.Portata,
                VolumeCarico                       = mm.VolumeCarico,
                AltezzaInterna                     = mm.AltezzaInterna,
                LarghezzaInterna                   = mm.LarghezzaInterna,
                LunghezzaInterna                   = mm.LunghezzaInterna,
                SegmentoDescrizione                = sm.Descrizione,
                CodiceSegmento                     = sm.CodiceSegmento,
                ModelloMezzoIDErs                  = sm.ModelloMezzoIDErs,
                IndexPricing                       = sm.IndexPricing,
                SegmentoModelloClasseID            = smc.SegmentoModelloClasseID,
                SegmentoModelloClasseIDDescrizione = smc.Descrizione,
                AltezzaEsterna                     = mm.AltezzaEsterna,
                LarghezzaEsterna                   = mm.LarghezzaEsterna,
                LunghezzaEsterna                   = mm.LunghezzaEsterna,
                Passo                              = mm.Passo,
                LarghezzaPassaruote                = mm.LarghezzaPassaruote,
                NumeroPostiCarrozzina              = mm.NumeroPostiCarrozzina,
                NumeroPostiMobility                = mm.NumeroPostiMobility,
                PedanaSollevatriceDoppioBraccio    = mm.PedanaSollevatriceDoppioBraccio,
                DescrizioneMobilitySitoWeb_ITA     = mm.DescrizioneMobilitySitoWeb_ITA,
                DescrizioneMobilitySitoWeb_ENG     = mm.DescrizioneMobilitySitoWeb_ENG
            };

        var result = await query.AsNoTracking().ToListAsync(cancellationToken);

        if (mezzoSpeciale.HasValue && specialSet is not null)
        {
            result = mezzoSpeciale.Value
                ? result.Where(r => specialSet.Contains(r.CodiceSegmento!)).ToList()
                : result.Where(r => !specialSet.Contains(r.CodiceSegmento!)).ToList();
        }

        return result;
    }
}
