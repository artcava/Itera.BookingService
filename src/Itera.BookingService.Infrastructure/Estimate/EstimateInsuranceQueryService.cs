using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Itera.BookingService.Infrastructure.Estimate;

internal sealed class EstimateInsuranceQueryService(LegacyDbContext dbContext) : IEstimateInsuranceQueryService
{
    public async Task<List<InsuranceExtraDto>> GetInsuranceExtraAsync(
        string segmentCode,
        DateTime dateFrom,
        DateTime dateTo,
        int rentalDays,
        int catalogId,
        CancellationToken cancellationToken)
    {
        var today = DateTime.Today;

        var franchigie = await (
            from lf in dbContext.ListinoFranchigie
            // INNER JOIN ListinoFranchigiaTipologia  (lo stato e StatoInclusione vivono qui)
            join lft in dbContext.ListinoFranchigiaTipologie
                on new { lf.ListinoID, lf.TipologiaFranchigiaID }
                equals new { lft.ListinoID, lft.TipologiaFranchigiaID }
            // INNER JOIN TipologiaFranchigia
            join tf in dbContext.TipologieFranchigia
                on lft.TipologiaFranchigiaID equals tf.TipologiaFranchigiaID
            // INNER JOIN StatoElemento  (Tipologia = 'GENERIC')
            join se in dbContext.StatiElemento
                    .Where(s => s.Tipologia == "GENERIC")
                on lft.StatoID equals se.Codice
            // LEFT JOIN TipologiaFranchigiaCategoria
            from tfc in dbContext.TipologieFranchigiaCategoria
                .Where(c => c.TipologiaFranchigiaCategoriaID == tf.TipologiaFranchigiaCategoriaID)
                .DefaultIfEmpty()

            // ------ filtri ------
            where lf.ListinoID == catalogId
               && lf.CodiceSegmento == segmentCode
               // validità della riga di listino
               && lf.ValidaDal <= today
               && (lf.ValidaAl == null || lf.ValidaAl >= today)
               // soloAttive = true (StatoID == 1 in ListinoFranchigiaTipologia)
               && lft.StatoID == 1
               // filtro core della view originale
               && tf.TipologiaVoceFatturaID != null
               // filtraFranchigieTariffa = true (default)
               && (lf.MinGiorniApplicabilita == null || lf.MinGiorniApplicabilita <= rentalDays)
               && (lf.MaxGiorniApplicabilita == null || lf.MaxGiorniApplicabilita >= rentalDays)

            // ordinamento per priorità tipologia (coerente con il legacy)
            orderby tf.Priorita

            select new InsuranceExtraDto
            {
                InsuranceExtraID      = lf.ListinoFranchigiaID,
                InsuranceExtraDescr   = tf.Descrizione,
                InsuranceExtra        = lf.TipologiaFranchigiaID,
                InsuranceExtraWithoutIVA = lf.CostoCoperturaExtra.ToString("F2"),
                Type                  = lft.StatoInclusione,
                CategoryID            = tfc != null ? tfc.Codice : null
            }
        )
        .AsNoTracking()
        .ToListAsync(cancellationToken);

        return franchigie;    
    }
}