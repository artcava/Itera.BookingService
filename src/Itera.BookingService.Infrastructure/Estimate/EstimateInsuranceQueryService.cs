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

internal sealed class EstimateAmountTokenQueryService(LegacyDbContext dbContext) : IEstimateAmountTokenQueryService
{
    public async Task<EstimateTokenValidationResult> ValidateEstimateTokenAsync(
        string estimateToken,
        int tokenValidPeriodSeconds,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(estimateToken, out var tokenGuid))
            return new EstimateTokenValidationResult(-3, null);

        var tokenRow = await dbContext.WsTokenPreventivi
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Token == tokenGuid, cancellationToken);

        if (tokenRow is null)
            return new EstimateTokenValidationResult(-3, null);

        var now = DateTime.Now;
        var isValid = tokenRow.DataCreazione == tokenRow.DataUltimaModifica
                      && tokenRow.DataUltimaModifica.AddSeconds(tokenValidPeriodSeconds) >= now;

        if (isValid)
        {
            var snapshot = new EstimateTokenSnapshot(
                WsUserId: tokenRow.WsUserID,
                FilialeId: tokenRow.FilialeID,
                FilialeDestinazioneId: tokenRow.FilialeIDDestinazione,
                DataFromPreventivo: tokenRow.DataFromPreventivo,
                DataToPreventivo: tokenRow.DataToPreventivo,
                Giorni: tokenRow.Giorni,
                ListinoId: tokenRow.ListinoID,
                CodiceDurata: tokenRow.CodiceDurata,
                CodiceCategoria: tokenRow.CodiceCategoria,
                ObjectDynParam: tokenRow.ObjectDynParam,
                ObjectEstimate: tokenRow.ObjectEstimate,
                VoucherCliente: tokenRow.VoucherCliente);

            return new EstimateTokenValidationResult(0, snapshot);
        }

        if (tokenRow.DataCreazione < tokenRow.DataUltimaModifica)
            return new EstimateTokenValidationResult(-1, null);

        if (tokenRow.DataUltimaModifica.AddSeconds(tokenValidPeriodSeconds) < now)
            return new EstimateTokenValidationResult(-2, null);

        return new EstimateTokenValidationResult(-3, null);
    }

    public async Task<bool> AcceptsNonSellableSegmentAsync(
        int wsUserId,
        CancellationToken cancellationToken)
    {
        var value = await dbContext.WsUsers
            .Where(u => u.WsUserID == wsUserId)
            .Select(u => u.AccettaSegmentoNonVendibile)
            .FirstOrDefaultAsync(cancellationToken);

        return value.GetValueOrDefault();
    }

    public async Task<Dictionary<short, string>> GetAccessoryCodesByIdsAsync(
        IReadOnlyCollection<short> accessoryIds,
        CancellationToken cancellationToken)
    {
        if (accessoryIds.Count == 0)
            return [];

        return await dbContext.AccessorioTipologie
            .Where(a => accessoryIds.Contains(a.AccessorioTipologiaID))
            .Select(a => new { a.AccessorioTipologiaID, a.Codice })
            .ToDictionaryAsync(x => x.AccessorioTipologiaID, x => x.Codice, cancellationToken);
    }

    public async Task<List<EstimateInsuranceOption>> GetInsuranceOptionsAsync(
        string segmentCode,
        int rentalDays,
        int catalogId,
        DateTime dateFrom,
        DateTime dateTo,
        CancellationToken cancellationToken)
    {
        var today = DateTime.Today;

        return await (
            from lf in dbContext.ListinoFranchigie
            join lft in dbContext.ListinoFranchigiaTipologie
                on new { lf.ListinoID, lf.TipologiaFranchigiaID }
                equals new { lft.ListinoID, lft.TipologiaFranchigiaID }
            join tf in dbContext.TipologieFranchigia
                on lft.TipologiaFranchigiaID equals tf.TipologiaFranchigiaID
            where lf.ListinoID == catalogId
               && lf.CodiceSegmento == segmentCode
               && lf.ValidaDal <= today
               && (lf.ValidaAl == null || lf.ValidaAl >= today)
               && lft.StatoID == 1
               && tf.TipologiaVoceFatturaID != null
               && (lf.MinGiorniApplicabilita == null || lf.MinGiorniApplicabilita <= rentalDays)
               && (lf.MaxGiorniApplicabilita == null || lf.MaxGiorniApplicabilita >= rentalDays)
            select new EstimateInsuranceOption(lf.ListinoFranchigiaID, tf.TipologiaFranchigiaID)
        )
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    }

    public async Task<short?> GetCurrentIvaIdAsync(CancellationToken cancellationToken)
    {
        var iva = await dbContext.Iva
            .AsNoTracking()
            .Where(i => i.Sistema)
            .OrderByDescending(i => i.ValidaDal)
            .FirstOrDefaultAsync(cancellationToken);

        return iva?.IvaId;
    }

    public async Task<decimal?> GetCurrentIvaPercentageAsync(CancellationToken cancellationToken)
    {
        var iva = await dbContext.Iva
            .AsNoTracking()
            .Where(i => i.Sistema)
            .OrderByDescending(i => i.ValidaDal)
            .FirstOrDefaultAsync(cancellationToken);

        return iva?.Percentuale;
    }
}