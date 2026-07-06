using Itera.BookingService.Application.Estimate.Abstractions;
using Itera.BookingService.Contracts.Estimate;
using Itera.BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Itera.BookingService.Infrastructure.Estimate;

public sealed class ProvinceQueryService(
    LegacyDbContext db,
    ILogger<ProvinceQueryService> logger) : IProvinceQueryService
{
    public async Task<List<GetProvince>> GetProvinceAsync(CancellationToken ct = default)
    {
        var result = await db.Province
            .OrderBy(p => p.Denominazione)
            .Select(p => new GetProvince
            {
                CodiceProvincia      = p.SiglaAutomobilistica,
                DescrizioneProvincia = p.Denominazione
            })
            .AsNoTracking()
            .ToListAsync(ct);

        logger.LogInformation("GetProvince: restituite {Count} province dal DB", result.Count);

        return result;
    }
}
