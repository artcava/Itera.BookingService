using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Infrastructure.Persistence;
using Itera.BookingService.Infrastructure.Persistence.KeylessTypes;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Itera.BookingService.Infrastructure.Estimate;

internal sealed class EstimateAccessoryQueryService(
    LegacyDbContext dbContext,
    IMapper mapper) : IEstimateAccessoryQueryService
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
        // GetAccessoriDettaglio è la UDF del legacy — riceve i parametri
        // di filtro e restituisce le righe già filtrate per filiale/brand/segmento/canale.
        // Non usiamo Migrations: la UDF esiste già sullo schema legacy.
        var rows = await dbContext
            .Set<GetAccessoriDettaglioResult>()
            .FromSqlRaw(
                """
                SELECT * FROM dbo.GetAccessoriDettaglio(
                    {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})
                """,
                "C",           // momentoVendibilita = CREAZIONE
                branchId,
                segmentCode ?? (object)DBNull.Value,
                3,             // canale Web = 3
                dateFrom,
                brandId,
                true,          // soloAttivi
                (object?)null) // dataCheckValidita = today (null → default in UDF)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (rows.Count == 0)
            return [];

        // Recupero tariffe: join su AccessorioFiliale per filtro filiale e
        // su tabella Iva per importo ivato.
        // Nessuna logica di tariffa inline: questa è una query di sola lettura.
        var accessorioIds = rows.Select(r => r.AccessorioTipologiaID).Distinct().ToList();

        var tariffe = await dbContext.TariffaRdvs
            .Where(t =>
                accessorioIds.Contains(t.AccessorioTipologiaId) &&
                t.ListinoId == catalogId &&
                t.GiorniNoleggio == rentalDays &&
                (accordoCommercialeId == null || t.AccordoCommercialeId == null || t.AccordoCommercialeId == accordoCommercialeId) &&
                t.DataInizio <= dateFrom && t.DataFine >= dateFrom)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var tariffaLookup = tariffe
            .GroupBy(t => t.AccessorioTipologiaId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(t => t.Priorita).First());

        var ivaPercentuale = await dbContext.Iva
            .Where(i => i.IvaId == ivaId)
            .Select(i => i.Percentuale)
            .FirstOrDefaultAsync(cancellationToken);

        return rows
            .Where(r => tariffaLookup.ContainsKey(r.AccessorioTipologiaID))
            .Select(r =>
            {
                var tariffa = tariffaLookup[r.AccessorioTipologiaID];
                var imponibile = tariffa.ImportoCalcolato;
                var ivato = imponibile * (1 + ivaPercentuale / 100m);
                return new AccessoryBookingDto
                {
                    AccessoryId        = r.AccessorioTipologiaID,
                    InvoiceLineTypeId  = r.TipologiaVoceFatturaID,
                    Code               = r.Codice,
                    Description        = r.DescrizioneTipologiaVoceFattura,
                    CategoryId         = r.AccessorioCategoriaID,
                    CategoryCode       = r.CodiceCategoria,
                    CategoryDescription= r.DescrizioneCategoria,
                    Mandatory          = r.Obbligatorio,
                    Preselected        = r.Preselezionato,
                    PrepaidWeb         = r.PrepagamentoWeb,
                    Amount             = imponibile,
                    AmountVat          = ivato,
                    VatId              = ivaId,
                    MomentOfSale       = r.MomentoVendibilita
                };
            })
            .ToList();
    }
}