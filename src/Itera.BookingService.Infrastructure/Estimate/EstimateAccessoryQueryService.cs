using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Infrastructure.Persistence;
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
        short ivaId,
        int? accordoCommercialeId,
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
                dbContext.AccessorioFiliali.Where(af => af.FilialeId == branchId),
                act => act.AccessorioTipologiaId,
                af  => af.AccessorioTipologiaId,
                (act, af) => act)
            // INNER JOIN AccessorioSegmento
            .Join(
                dbContext.AccessorioSegmenti
                    .Where(ass => segmentiList == null || segmentiList.Count == 0
                                  || segmentiList.Contains(ass.CodiceSegmento)),
                act => act.AccessorioTipologiaId,
                ass => ass.AccessorioTipologiaId,
                (act, ass) => new { Accessorio = act, ass.CodiceSegmento })
            // Filtro brand: BrandID IS NULL oppure brandID passato nullo oppure corrispondente
            .Where(x => x.Accessorio.BrandId == null || x.Accessorio.BrandId == brandId)
            // soloAttivi = true
            .Where(x => x.Accessorio.StatoId == 1)
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
                x   => x.Accessorio.TipologiaVoceFatturaId,
                tvf => tvf.TipologiaVoceFatturaId,
                (x, tvf) => new { x.Accessorio, x.CodiceSegmento, TipologiaVoceFattura = tvf })
            // JOIN lookup: AccessorioCategoria (INNER nella VwAccessori)
            .Join(
                dbContext.AccessorioCategorie,
                x   => x.Accessorio.AccessorioCategoriaId,
                acc => acc.AccessorioCategoriaId,
                (x, acc) => new { x.Accessorio, x.CodiceSegmento, x.TipologiaVoceFattura, Categoria = acc })
            // LEFT JOIN Iva (LEFT OUTER nella VwAccessori)
            .GroupJoin(
                dbContext.Ive,
                x   => x.Accessorio.IvaId,
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
            .OrderBy(x => x.Accessorio.AccessorioCategoriaId)
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

        var accessorioTipologiaIds = accessori
            .Select(x => x.Accessorio.AccessorioTipologiaId)
            .Distinct()
            .ToList();

        var tariffe = await dbContext.TariffaRdvs
            .Where(t =>
                accessorioTipologiaIds.Contains(t.AccessorioTipologiaId) &&
                t.ListinoId == catalogId &&
                t.GiorniNoleggio == rentalDays &&
                t.DataInizio <= dateFrom &&
                t.DataFine   >= dateFrom &&
                (accordoCommercialeId == null || t.AccordoCommercialeId == null || t.AccordoCommercialeId == accordoCommercialeId) &&
                (brandId == 0 || t.BrandId == null || t.BrandId == brandId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Solo gli accessori per cui esiste una tariffa (come nel legacy: model.Where(m => accessoriConTariffa.Contains(...)))
        var tariffaLookup = tariffe
            .GroupBy(t => t.AccessorioTipologiaId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(t => t.Priorita).First());

        // ---------------------------------------------------------------
        // 3.  Recupera percentuale IVA corrente per calcolo importo ivato.
        //     Nel legacy: IvaBL.AddIva(ivaID, importo) = importo * (1 + %)
        // ---------------------------------------------------------------

        var ivaPercentuale = await dbContext.Ive
            .Where(i => i.IvaId == ivaId)
            .Select(i => i.Percentuale)
            .FirstOrDefaultAsync(cancellationToken);

        // ---------------------------------------------------------------
        // 4.  Proiezione verso AccessoryBookingDto.
        //     Replica la mappatura WsGetAccessorioPrenotazione del legacy.
        //     QuantityMax viene gestito al livello Application/Service
        //     (nel legacy era in WsPreventivoBL.GetQuantitaMaxAccessori).
        // ---------------------------------------------------------------

        return accessori
            .Where(x => tariffaLookup.ContainsKey(x.Accessorio.AccessorioTipologiaId))
            .Select(x =>
            {
                var tariffa    = tariffaLookup[x.Accessorio.AccessorioTipologiaId];
                var imponibile = tariffa.ImportoCalcolato;
                var ivato      = imponibile * (1m + ivaPercentuale / 100m);

                return new AccessoryBookingDto
                {
                    AccessoryId          = x.Accessorio.AccessorioTipologiaId,
                    InvoiceLineTypeId    = x.Accessorio.TipologiaVoceFatturaId,
                    Code                 = x.Accessorio.Codice,
                    Description          = x.TipologiaVoceFattura.Descrizione,
                    CategoryId           = x.Accessorio.AccessorioCategoriaId,
                    CategoryCode         = x.Categoria.Codice,
                    CategoryDescription  = x.Categoria.Descrizione,
                    Mandatory            = x.Accessorio.Obbligatorio,
                    Preselected          = x.Accessorio.Preselezionato,
                    PrepaidWeb           = x.Accessorio.PrepagamentoWeb,
                    Amount               = imponibile,
                    AmountVat            = ivato,
                    VatId                = x.Accessorio.IvaId ?? ivaId,
                    MomentOfSale         = x.Accessorio.MomentoVendibilita,
                    SegmentCode          = x.CodiceSegmento
                };
            })
            .ToList();
    }
}
