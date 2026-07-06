using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Infrastructure.Estimate;

public sealed class NationQueryService(
    LegacyDbContext               db,
    ILogger<NationQueryService>   logger) : INationQueryService
{
    public async Task<List<Nazione>> GetNationsAsync(string? language, CancellationToken ct = default)
    {
        var isEnglish = string.Equals(language, "en",  StringComparison.OrdinalIgnoreCase)
                     || string.Equals(language, "2",   StringComparison.OrdinalIgnoreCase);

        var result = await db.StatiEsteri
            .Where(n => n.ContinenteID != 6)           // escludiApolidi=true (default legacy GetNation)
            .OrderBy(n => isEnglish ? n.DescrizioneInternazionale : n.DescrizioneItaliana)
            .Select(n => new Nazione
            {
                CodiceNazione      = n.CodiceNazione,
                DescrizioneNazione = isEnglish ? n.DescrizioneInternazionale : n.DescrizioneItaliana
            })
            .AsNoTracking()
            .ToListAsync(ct);

        logger.LogInformation("GetNations: restituite {Count} nazioni dal DB", result.Count);

        return result;
    }
}