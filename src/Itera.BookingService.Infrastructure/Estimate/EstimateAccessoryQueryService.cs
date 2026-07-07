using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Infrastructure.Estimate.Mapping;
using Itera.BookingService.Infrastructure.Persistence;
using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Itera.BookingService.Infrastructure.Estimate;

/// <summary>
/// Implementa la lettura degli accessori prenotabili via canale Web
/// replicando la logica legacy di AccessoriHelper.GetAccessoriTipologiaDettaglio
/// e WsPreventivoBL.GetAccessoryBooking, senza ricorrere alla UDF
/// GetAccessoriDettaglio né alla view VwAccessori.
///
/// La UDF GetAccessoriDettaglio accettava solo (filialeID, segmentoMulti, brandID):
/// i filtri su StatoID, canale, data validità e momentoVendibilita erano
/// applicati in C# dal helper legacy dopo la chiamata alla UDF.
/// Qui ripetiamo gli stessi filtri inline nella query EF.
/// </summary>
internal sealed class EstimateAccessoryQueryService(
    LegacyDbContext dbContext) : IEstimateAccessoryQueryService
{
    public async Task<List<AccessoryBookingDto>> GetAccessoryBookingAsync(
        short brandId,
        int branchId,
        int branchDestinationId,
        int catalogId,
        int rentalDays,
        DateTime dateFrom,
        DateTime dateTo,
        string? categoryId,
        string? segmentCode,
        CancellationToken cancellationToken)
    {
        // ---------------------------------------------------------------
        // 1.  Replica dei JOIN della TVF GetAccessoriDettaglio +
        //     filtri del helper legacy applicati in-query anziché in-memory.
        //
        // TVF originale:
        //   FROM VwAccessori VW
        //   INNER JOIN AccessorioFiliale AF  ON VW.AccessorioTipologiaID = AF.AccessorioTipologiaID
        //                                    AND AF.FilialeID = @filialeID
        //   INNER JOIN AccessorioSegmento ASS ON VW.AccessorioTipologiaID = ASS.AccessorioTipologiaID
        //   WHERE (VW.BrandID IS NULL OR (@brandID IS NULL OR VW.BrandID = @brandID))
        //     AND (@segmentoMulti IS NULL OR ASS.CodiceSegmento IN (...))
        //
        // Filtri canale Web applicati in C# dal helper legacy:
        //   StatoID == 1 (soloAttivi = true per GetAccessoryBooking)
        //   VendibilitaWeb == true
        //   DataInizioValidita <= dataCheckValidita (oggi)
        //   DataFineValidita   >= dataCheckValidita (oggi) oppure NULL
        //   MomentoVendibilita == momentoVendibilita oppure NULL/empty
        //     (momentoVendibilita = ImportiPreventivoBL.MOMENTOIMPORTO_CREAZIONE = "C")
        // ---------------------------------------------------------------

        var dataCheckValidita = DateTime.Today;
        const string momentoCreazione = "C"; // MOMENTOIMPORTO_CREAZIONE

        var segmentiList = segmentCode?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        var accessori = await dbContext.AccessorioTipologie
            // INNER JOIN AccessorioFiliale  (il JOIN della TVF filtra per filiale)
            .Join(
                dbContext.AccessorioFiliali.Where(af => af.FilialeID == branchId),
                act => act.AccessorioTipologiaID,
                af  => af.AccessorioTipologiaID,
                (act, af) => act)
            // INNER JOIN AccessorioSegmento
            .Join(
                dbContext.AccessorioSegmenti
                    .Where(ass => segmentiList == null || segmentiList.Count == 0
                                  || segmentiList.Contains(ass.CodiceSegmento)),
                act => act.AccessorioTipologiaID,
                ass => ass.AccessorioTipologiaID,
                (act, ass) => new { Accessorio = act, ass.CodiceSegmento })
            // Filtro brand: BrandID IS NULL oppure brandID passato nullo oppure corrispondente
            .Where(x => x.Accessorio.BrandID == null || x.Accessorio.BrandID == brandId)
            // soloAttivi = true
            .Where(x => x.Accessorio.StatoID == 1)
            // canale Web
            .Where(x => x.Accessorio.VendibilitaWeb == true)
            // data validità
            .Where(x => (x.Accessorio.DataInizioValidita == null || x.Accessorio.DataInizioValidita.Value <= dataCheckValidita)
                     && (x.Accessorio.DataFineValidita   == null || x.Accessorio.DataFineValidita.Value   >= dataCheckValidita))
            // momento vendibilità: null/empty = vendibile in tutti i momenti
            .Where(x => string.IsNullOrEmpty(x.Accessorio.MomentoVendibilita)
                     || x.Accessorio.MomentoVendibilita == momentoCreazione)
            // JOIN lookup: TipologiaVoceFattura (INNER nella VwAccessori)
            .Join(
                dbContext.TipologieVoceFattura,
                x   => x.Accessorio.TipologiaVoceFatturaID,
                tvf => tvf.TipologiaVoceFatturaID,
                (x, tvf) => new { x.Accessorio, x.CodiceSegmento, TipologiaVoceFattura = tvf })
            // JOIN lookup: AccessorioCategoria (INNER nella VwAccessori)
            .Join(
                dbContext.AccessorioCategorie,
                x   => x.Accessorio.AccessorioCategoriaID,
                acc => acc.AccessorioCategoriaID,
                (x, acc) => new { x.Accessorio, x.CodiceSegmento, x.TipologiaVoceFattura, Categoria = acc })
            // LEFT JOIN Iva (LEFT OUTER nella VwAccessori)
            .GroupJoin(
                dbContext.Iva,
                x   => x.Accessorio.IvaID,
                iva => iva.IvaId,
                (x, ivaGroup) => new { x, ivaGroup })
            .SelectMany(
                g => g.ivaGroup.DefaultIfEmpty(),
                (g, iva) => new
                {
                    g.x.Accessorio,
                    g.x.CodiceSegmento,
                    g.x.TipologiaVoceFattura,
                    g.x.Categoria,
                    Iva = iva
                })
            // Ordinamento identico al legacy: per categoria poi per descrizione TVF
            .OrderBy(x => x.Accessorio.AccessorioCategoriaID)
            .ThenBy(x => x.TipologiaVoceFattura.Descrizione)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (accessori.Count == 0)
            return [];

        // ---------------------------------------------------------------
        // 2.  Recupera le tariffe per gli accessori trovati.
        //     Replica la chiamata TariffeBL.GetTariffeRdvImporto del legacy.
        //     Filtriamo per listinoID, giorniNoleggio, dataInizioNoleggio,
        //     accordoCommerciale e brandID.
        //     In caso di più righe per lo stesso accessorio prendiamo
        //     quella a priorità più alta (stesso criterio del legacy).
        // ---------------------------------------------------------------

        var dataValidita = dateFrom.Date;

        var accessorioTipologiaIds = accessori
            .Select(x => x.Accessorio.AccessorioTipologiaID)
            .Distinct()
            .ToList();

        var tariffarioAccordoId = await dbContext.AccordiCommercialiListino
            .Where(x =>
                x.ListinoID == catalogId &&
                x.PeriodoValiditaDa <= dataValidita &&
                x.PeriodoValiditaA >= dataValidita)
            .Select(x => (int?)x.TariffarioID)
            .FirstOrDefaultAsync(cancellationToken);

        var tariffarioId = await (
            from li in dbContext.Listini
            join ta in dbContext.Tariffari on li.TariffarioId equals ta.TariffarioID
            where li.ListinoId == catalogId
                && (ta.BrandID == null || ta.BrandID == brandId)
            select tariffarioAccordoId ?? li.TariffarioId
        ).FirstOrDefaultAsync(cancellationToken);

        if (tariffarioId == 0)
            return [];

        var candidateTariffe = await dbContext.TariffeRdv
            .Where(t =>
                t.TariffarioID == tariffarioId &&
                accessorioTipologiaIds.Contains(t.AccessorioTipologiaID) &&
                t.ImportoFisso != null &&
                t.ImportoFisso >= 0 &&
                t.DataStart <= dataValidita &&
                t.DataEnd >= dataValidita &&
                t.BreakEven >= rentalDays)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var selectedTariffe = candidateTariffe
            .GroupBy(t => t.AccessorioTipologiaID)
            .Select(g => g.OrderBy(t => t.BreakEven).First())
            .Where(t =>
                t.MinGiorniApplicabilita <= rentalDays &&
                (t.TipoImporto != "U" || t.MaxGiorniApplicabilita >= rentalDays))
            .ToList();

        var calculatedTariffaLookup = selectedTariffe
            .ToDictionary(
                t => t.AccessorioTipologiaID,
                t => BuildCalculatedTariffa(
                    t,
                    rentalDays,
                    momentoCreazione,
                    BuildPeriodoCompetenzaDefault(),
                    accessori.First(x => x.Accessorio.AccessorioTipologiaID == t.AccessorioTipologiaID).Accessorio.MomentoVendibilita));

        // ---------------------------------------------------------------
        // 3.  Recupera percentuale IVA corrente per calcolo importo ivato.
        //     Nel legacy: IvaBL.AddIva(ivaID, importo) = importo * (1 + %)
        // ---------------------------------------------------------------

        var iva = await dbContext.Iva
            .Where(i => i.Sistema)
            // .Select(i => i.Percentuale)
            .FirstOrDefaultAsync(cancellationToken);
        if(iva == null)
            throw new InvalidOperationException("Nessuna aliquota IVA di sistema trovata.");

        // ---------------------------------------------------------------
        // 4.  Proiezione verso AccessoryBookingDto.
        //     Replica la mappatura WsGetAccessorioPrenotazione del legacy.
        //     QuantityMax viene gestito al livello Application/Service
        //     (nel legacy era in WsPreventivoBL.GetQuantitaMaxAccessori).
        // ---------------------------------------------------------------

        return accessori
            .Where(x => calculatedTariffaLookup.ContainsKey(x.Accessorio.AccessorioTipologiaID))
            .Select(x =>
            {
                var tariffa    = calculatedTariffaLookup[x.Accessorio.AccessorioTipologiaID];
                var imponibile = tariffa.ImportoCalcolato;
                var ivato      = imponibile * (1m + iva.Percentuale / 100m);

                return new AccessoryBookingDto
                {
                    AccessoryId          = x.Accessorio.AccessorioTipologiaID,
                    InvoiceLineTypeId    = x.Accessorio.TipologiaVoceFatturaID,
                    Code                 = x.Accessorio.Codice,
                    Description          = x.TipologiaVoceFattura.Descrizione,
                    CategoryId           = x.Accessorio.AccessorioCategoriaID,
                    CategoryCode         = x.Categoria.Codice,
                    CategoryDescription  = x.Categoria.Descrizione,
                    Mandatory            = x.Accessorio.Obbligatorio,
                    Preselected          = x.Accessorio.Preselezionato,
                    PrepaidWeb           = x.Accessorio.PrepagamentoWeb,
                    Amount               = imponibile,
                    AmountVat            = ivato,
                    VatId                = x.Accessorio.IvaID ?? iva.IvaId,
                    MomentOfSale         = x.Accessorio.MomentoVendibilita
                };
            })
            .ToList();
    }
    private static TariffaRdvBase BuildCalculatedTariffa(
        TariffaRdv entity,
        int giorniNoleggio,
        string? momentoVendibilitaPreventivo,
        PeriodoCompetenza periodoCompetenza,
        string? momentoVendibilitaAccessorio)
    {
        TariffaRdvBase tariffa = entity.TipoImporto switch
        {
            "F" => new ImportoForfettario(),
            "X" => new ImportoForfettarioNonRipetuto(),
            "U" => new ImportoUnitario(),
            _ => throw new InvalidOperationException($"TipoImporto non gestito: {entity.TipoImporto}")
        };

        tariffa.GiorniNoleggio = giorniNoleggio;
        tariffa.MomentoVendibilitaPreventivo = momentoVendibilitaPreventivo;
        tariffa.MomentoVendibilitaAccessorio = momentoVendibilitaAccessorio;
        tariffa.PeriodoCompetenza = periodoCompetenza;

        tariffa.TariffarioID = entity.TariffarioID;
        tariffa.AccessorioTipologiaID = entity.AccessorioTipologiaID;
        tariffa.DataStart = entity.DataStart;
        tariffa.DataEnd = entity.DataEnd;
        tariffa.BreakEven = entity.BreakEven;
        tariffa.MinGiorniApplicabilita = entity.MinGiorniApplicabilita;
        tariffa.MaxGiorniApplicabilita = entity.MaxGiorniApplicabilita;
        tariffa.Percentuale = entity.Percentuale;
        tariffa.ImportoFisso = entity.ImportoFisso ?? 0m;
        tariffa.ImportoGiornoExtra = entity.ImportoGiornoExtra;
        tariffa.ImportoMinAddebitabile = entity.ImportoMinAddebitabile;
        tariffa.ImportoMaxAddebitabile = entity.ImportoMaxAddebitabile;
        tariffa.MaxGiorniAddebitabili = entity.MaxGiorniAddebitabili;
        tariffa.Tolleranza = entity.Tolleranza;
        tariffa.StatoInclusione = entity.StatoInclusione;
        tariffa.Incasso = entity.Incasso;

        return tariffa;
    }
    private static PeriodoCompetenza BuildPeriodoCompetenzaDefault()
    {
        return new PeriodoCompetenza();
    }
}