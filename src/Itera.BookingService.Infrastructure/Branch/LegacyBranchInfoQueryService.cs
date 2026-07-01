using Itera.BookingService.Application.Abstractions;
using Itera.BookingService.Contracts.Legacy.Branch;
using Itera.BookingService.Infrastructure.Persistence;
using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

// Alias per disambiguare le entity EF dai DTO omonimi
using EfFilialeChiusuraExtra    = Itera.BookingService.Infrastructure.Persistence.Entities.FilialeChiusuraExtra;
using EfFilialeFasciaOrario     = Itera.BookingService.Infrastructure.Persistence.Entities.FilialeFasciaOrario;
using EfFilialeOrarioOperativo  = Itera.BookingService.Infrastructure.Persistence.Entities.FilialeOrarioOperativo;
using EfFilialeRiposoSettimanale = Itera.BookingService.Infrastructure.Persistence.Entities.FilialeRiposoSettimanale;

namespace Itera.BookingService.Infrastructure.Branch;

public sealed class LegacyBranchInfoQueryService(LegacyDbContext dbContext) : IBranchInfoQueryService
{
    public async Task<List<FilialeDto>> GetAllBranchesAsync(short brandId, bool getExtraData, bool getFilialiExtra, byte languageId, DateTime selectedDate, CancellationToken cancellationToken)
    {
        var rows = await BuildBaseBranchQuery(brandId)
            .OrderBy(x => x.FilialeID)
            .ToListAsync(cancellationToken);

        var allowedStates = getFilialiExtra ? new HashSet<short> { 1, 2 } : new HashSet<short> { 1 };
        var filteredRows = rows.Where(row => allowedStates.Contains(row.Stato ?? 0)).ToList();

        var translations = await ResolveTranslationsAsync(filteredRows, languageId, cancellationToken);
        var branchIds = filteredRows.Select(x => x.FilialeID).Distinct().ToList();

        var closingExtras = await dbContext.FilialeChiusureExtra.AsNoTracking()
            .Where(x => branchIds.Contains(x.FilialeID))
            .ToListAsync(cancellationToken);

        var timeSlots = await dbContext.FilialeFasceOrario.AsNoTracking()
            .Where(x => branchIds.Contains(x.FilialeID) && x.StatoID == 1)
            .OrderBy(x => x.FilialeID)
            .ThenBy(x => x.Ordinamento)
            .ToListAsync(cancellationToken);

        var operativeSlots = await dbContext.FilialeOrariOperativi.AsNoTracking()
            .Where(x => branchIds.Contains(x.FilialeID) && x.StatoID == 1)
            .OrderBy(x => x.FilialeID)
            .ThenBy(x => x.GiornoSettimana)
            .ThenBy(x => x.Ordinamento)
            .ToListAsync(cancellationToken);

        var restDays = await dbContext.FilialeRiposiSettimanali.AsNoTracking()
            .Where(x => branchIds.Contains(x.FilialeID))
            .ToListAsync(cancellationToken);

        var closingByBranch   = closingExtras.GroupBy(x => x.FilialeID).ToDictionary(g => g.Key, g => g.ToList());
        var slotsByBranch     = timeSlots.GroupBy(x => x.FilialeID).ToDictionary(g => g.Key, g => g.ToList());
        var operativeByBranch = operativeSlots.GroupBy(x => x.FilialeID).ToDictionary(g => g.Key, g => g.ToList());
        var restByBranch      = restDays.GroupBy(x => x.FilialeID).ToDictionary(g => g.Key, g => g.ToList());

        return filteredRows
            .Select(row => MapToFilialeDto(
                row,
                translations,
                getExtraData,
                languageId,
                closingByBranch.TryGetValue(row.FilialeID, out var c) ? c : [],
                slotsByBranch.TryGetValue(row.FilialeID, out var s) ? s : [],
                operativeByBranch.TryGetValue(row.FilialeID, out var o) ? o : [],
                restByBranch.TryGetValue(row.FilialeID, out var r) ? r : []))
            .ToList();
    }

    public async Task<FilialeDto?> GetInfoBranchAsync(short brandId, int branchId, bool getFilialiExtra, byte languageId, DateTime selectedDate, CancellationToken cancellationToken)
    {
        var row = await BuildBaseBranchQuery(brandId)
            .Where(x => x.FilialeID == branchId)
            .FirstOrDefaultAsync(cancellationToken);
        if (row is null)
        {
            return null;
        }

        var allowedStates = getFilialiExtra ? new HashSet<short> { 1, 2 } : new HashSet<short> { 1 };
        if (!allowedStates.Contains(row.Stato ?? 0))
        {
            return null;
        }

        var translations = await ResolveTranslationsAsync([row], languageId, cancellationToken);

        var closingExtras = await dbContext.FilialeChiusureExtra.AsNoTracking()
            .Where(x => x.FilialeID == row.FilialeID)
            .ToListAsync(cancellationToken);

        var timeSlots = await dbContext.FilialeFasceOrario.AsNoTracking()
            .Where(x => x.FilialeID == row.FilialeID && x.StatoID == 1)
            .OrderBy(x => x.Ordinamento)
            .ToListAsync(cancellationToken);

        var operativeSlots = await dbContext.FilialeOrariOperativi.AsNoTracking()
            .Where(x => x.FilialeID == row.FilialeID && x.StatoID == 1)
            .OrderBy(x => x.GiornoSettimana)
            .ThenBy(x => x.Ordinamento)
            .ToListAsync(cancellationToken);

        var restDays = await dbContext.FilialeRiposiSettimanali.AsNoTracking()
            .Where(x => x.FilialeID == row.FilialeID)
            .ToListAsync(cancellationToken);

        return MapToFilialeDto(row, translations, includeExtraData: true, languageId, closingExtras, timeSlots, operativeSlots, restDays);
    }

    private IQueryable<BranchRawRow> BuildBaseBranchQuery(short brandId)
    {
        return
            from f in dbContext.Filiali.AsNoTracking()
            join fc in dbContext.Franchises.AsNoTracking() on f.FilialeID equals fc.FranchiseID
            join ft in dbContext.FilialeTesti.AsNoTracking() on f.FilialeID equals ft.FilialeID
            join fa in dbContext.FilialeAree.AsNoTracking() on f.FilialeAreaID equals fa.FilialeAreaID
            join fb in dbContext.FilialeBrands.AsNoTracking() on f.FilialeID equals fb.FilialeID
            join fcfGroup in dbContext.FilialeClassificazioni.AsNoTracking() on f.FilialeClassificazioneID equals fcfGroup.FilialeClassificazioneID into fcfJoin
            from fcf in fcfJoin.DefaultIfEmpty()
            join pvGroup in dbContext.Province.AsNoTracking() on f.SiglaAutomobilistica equals pvGroup.SiglaAutomobilistica into pvJoin
            from pv in pvJoin.DefaultIfEmpty()
            join rgGroup in dbContext.Regioni.AsNoTracking() on pv.CodiceRegione equals rgGroup.CodiceRegione into rgJoin
            from rg in rgJoin.DefaultIfEmpty()
            where fb.BrandID == brandId
            select new BranchRawRow
            {
                FilialeID            = f.FilialeID,
                FranchiseID          = f.FranchiseID,
                Indirizzo            = f.Indirizzo,
                Cap                  = f.Cap,
                Citta                = f.Citta,
                Telefono             = f.Telefono,
                Fax                  = f.Fax,
                Email                = f.Email,
                CoordX               = f.CoordX,
                CoordY               = f.CoordY,
                SpnX                 = f.SpnX,
                SpnY                 = f.SpnY,
                Zoom                 = f.Zoom,
                Stato                = f.Stato,
                Parcheggio           = f.Parcheggio,
                RespCommerciale      = f.RespCommerciale,
                RespAmministrazione  = f.RespAmministrazione,
                KeyBox               = f.KeyBox,
                EsclusioneVal        = f.EsclusioneVal,
                FleetNonVendibile    = f.FleetNonVendibile,
                FilialeAreaID        = fa.FilialeAreaID,
                FilialeMacroAreaID   = fa.FilialeMacroAreaID,
                ClassificazioneID    = fcf != null ? (int?)fcf.FilialeClassificazioneID : null,
                Descrizione          = ft.Descrizione,
                OrariUfficio         = ft.OrariUfficio,
                OrariConsegna        = ft.OrariConsegna,
                Provincia            = pv != null ? pv.Denominazione : null,
                Regione              = rg != null ? rg.Denominazione : null
            };
    }

    private async Task<Dictionary<string, string?>> ResolveTranslationsAsync(IEnumerable<BranchRawRow> rows, byte languageId, CancellationToken cancellationToken)
    {
        var textKeys = rows
            .SelectMany(row => new[] { row.Descrizione, row.OrariUfficio, row.OrariConsegna })
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .Cast<string>()
            .ToList();

        if (textKeys.Count == 0)
        {
            return new Dictionary<string, string?>();
        }

        return await dbContext.Testi.AsNoTracking()
            .Where(t => t.LinguaID == languageId && t.Chiave != null && textKeys.Contains(t.Chiave))
            .ToDictionaryAsync(t => t.Chiave!, t => t.Valore, cancellationToken);
    }

    private static FilialeDto MapToFilialeDto(
        BranchRawRow row,
        IReadOnlyDictionary<string, string?> translations,
        bool includeExtraData,
        byte languageId,
        IReadOnlyList<EfFilialeChiusuraExtra> closingExtras,
        IReadOnlyList<EfFilialeFasciaOrario> timeSlots,
        IReadOnlyList<EfFilialeOrarioOperativo> operativeSlots,
        IReadOnlyList<EfFilialeRiposoSettimanale> restDays)
    {
        string? ResolveText(string? key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            return translations.TryGetValue(key, out var translated)
                ? translated
                : null;
        }

        var model = new FilialeDto
        {
            BranchID    = row.FilialeID,
            Description = ResolveText(row.Descrizione),
            FranchiseID = row.FranchiseID,
            KeyBox      = row.KeyBox ?? false,
            StateID     = row.Stato ?? 0,
            ExcludeVAL  = row.EsclusioneVal ?? false
        };

        if (includeExtraData)
        {
            var parkingDescription = ResolveParking(row.Parcheggio, languageId);
            var mapsLink = BuildMapsLink(row.CoordX, row.CoordY, row.SpnX, row.SpnY, row.Zoom);

            var closingMapped = closingExtras
                .SelectMany(item => BuildClosingDates(item))
                .OrderBy(x => x.Day)
                .ToList();

            var retireSlots = timeSlots
                .Where(x => x.TipologiaFasciaOrarioID == 1)
                .Select(MapFasciaOrarioDto)
                .ToList();

            var deliverySlots = timeSlots
                .Where(x => x.TipologiaFasciaOrarioID == 2)
                .Select(MapFasciaOrarioDto)
                .ToList();

            var timeTableBranch = operativeSlots
                .GroupBy(x => x.GiornoSettimana)
                .OrderBy(g => g.Key)
                .Select(g => new FilialeOrariOperativiDto
                {
                    Day            = g.Key,
                    DayDescription = DayDescription(g.Key, languageId),
                    TimeSlotDay    = g.Select(slot => new FasciaOraria
                    {
                        Start = TimeToString(slot.OraInizio, slot.MinutiInizio),
                        End   = TimeToString(slot.OraFine,   slot.MinutiFine)
                    }).ToList()
                })
                .ToList();

            var selected = timeTableBranch.FirstOrDefault(x => x.Day == (int)DateTime.Today.DayOfWeek);
            if (selected is not null)
            {
                selected.Date = DateTime.Today.ToString("yyyy-MM-dd");
            }

            model.ExtraData = new FilialeExtraDataDto
            {
                Address                  = row.Indirizzo,
                PostalCode               = row.Cap,
                City                     = row.Citta,
                Province                 = row.Provincia,
                Region                   = row.Regione,
                Telephone                = row.Telefono,
                Fax                      = row.Fax,
                Email                    = row.Email,
                CordX                    = row.CoordX,
                CordY                    = row.CoordY,
                SpnX                     = row.SpnX,
                SpnY                     = row.SpnY,
                MapsLink                 = mapsLink,
                TimeOfficeDescription    = ResolveText(row.OrariUfficio),
                TimeDeliveryDescription  = ResolveText(row.OrariConsegna),
                CommercialManager        = row.RespCommerciale,
                AdministrationManager    = row.RespAmministrazione,
                ParkingClient            = parkingDescription,
                TimeSlotRetire           = retireSlots,
                TimeSlotDelivery         = deliverySlots,
                WeeklyDayOfRest          = restDays
                    .Select(x => new FilialeRiposoSettimanaleDto
                    {
                        DayOfWeek   = x.GiornoSettimana,
                        Description = DayDescription(x.GiornoSettimana, languageId)
                    })
                    .ToList(),
                ClosingDayExtra    = closingMapped,
                FilialeAreaID      = row.FilialeAreaID,
                FilialeMacroAreaID = row.FilialeMacroAreaID,
                Location           = row.ClassificazioneID,
                RentalCar          = string.IsNullOrWhiteSpace(row.FleetNonVendibile)
            };

            model.TimeTableBranch     = timeTableBranch;
            model.TimeTableVariation  = [];
            model.TimeTableDaySelected = selected;
        }

        return model;
    }

    private static FilialeFasciaOrarioDto MapFasciaOrarioDto(EfFilialeFasciaOrario item)
    {
        return new FilialeFasciaOrarioDto
        {
            TimeSlot    = item.FilialeFasciaOrarioID,
            Description = $"{TimeToString(item.OraInizio, item.MinutiInizio)}-{TimeToString(item.OraFine, item.MinutiFine)}",
            ChangerDay  = item.ModificatoreGiorno,
            Selected    = item.DefaultSelectedWeb || item.DefaultSelected,
            DayPeriodID = item.PeriodoDelGiornoID,
            Start       = TimeToString(item.OraInizio, item.MinutiInizio),
            End         = TimeToString(item.OraFine,   item.MinutiFine)
        };
    }

    private static List<FilialeGiornoChiusuraExtraDto> BuildClosingDates(EfFilialeChiusuraExtra item)
    {
        var result = new List<FilialeGiornoChiusuraExtraDto>();
        var year = item.Anno ?? DateTime.Today.Year;
        if (DateTime.TryParse($"{year:D4}-{item.Mese:D2}-{item.Giorno:D2}", out var date))
        {
            result.Add(new FilialeGiornoChiusuraExtraDto
            {
                DayClosureExtraID = item.FilialeChiusuraExtraID,
                Day               = date,
                DayFormatted      = date.ToString("yyyy-MM-dd")
            });

            if (!item.Anno.HasValue && DateTime.TryParse($"{year + 1:D4}-{item.Mese:D2}-{item.Giorno:D2}", out var nextDate))
            {
                result.Add(new FilialeGiornoChiusuraExtraDto
                {
                    DayClosureExtraID = item.FilialeChiusuraExtraID,
                    Day               = nextDate,
                    DayFormatted      = nextDate.ToString("yyyy-MM-dd")
                });
            }
        }

        return result;
    }

    private static string? ResolveParking(string? raw, byte languageId)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return raw;
        }

        var parts = raw.Split('|');
        if (parts.Length != 2)
        {
            return raw;
        }

        return languageId == 2 ? parts[1] : parts[0];
    }

    private static string? BuildMapsLink(double? coordX, double? coordY, double? spnX, double? spnY, short? zoom)
    {
        if (!coordX.HasValue || !coordY.HasValue)
        {
            return null;
        }

        var z  = zoom ?? 14;
        var sx = spnX ?? 0;
        var sy = spnY ?? 0;
        return $"https://maps.google.com/maps/ms?msa=0&msid=216487878552902451104.0004b40d7836e51178f7f&ie=UTF8&t=m&ll={coordX.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)},{coordY.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}&spn={sx.ToString(System.Globalization.CultureInfo.InvariantCulture)},{sy.ToString(System.Globalization.CultureInfo.InvariantCulture)}&z={z}&output=embed";
    }

    private static string DayDescription(int dayOfWeek, byte languageId)
    {
        var culture = languageId == 2
            ? System.Globalization.CultureInfo.GetCultureInfo("en-US")
            : System.Globalization.CultureInfo.GetCultureInfo("it-IT");

        return culture.DateTimeFormat.GetDayName((DayOfWeek)dayOfWeek).ToLowerInvariant();
    }

    private static string TimeToString(int hour, int minute)
    {
        return new TimeSpan(hour, minute, 0).ToString("hh\\:mm");
    }

    private sealed class BranchRawRow
    {
        public int FilialeID { get; init; }
        public int FranchiseID { get; init; }
        public string? Indirizzo { get; init; }
        public string? Cap { get; init; }
        public string? Citta { get; init; }
        public string? Telefono { get; init; }
        public string? Fax { get; init; }
        public string? Email { get; init; }
        public double? CoordX { get; init; }
        public double? CoordY { get; init; }
        public double? SpnX { get; init; }
        public double? SpnY { get; init; }
        public short? Stato { get; init; }
        public string? Parcheggio { get; init; }
        public string? RespCommerciale { get; init; }
        public string? RespAmministrazione { get; init; }
        public bool? KeyBox { get; init; }
        public bool? EsclusioneVal { get; init; }
        public int FilialeAreaID { get; init; }
        public int FilialeMacroAreaID { get; init; }
        public int? ClassificazioneID { get; init; }
        public string? Descrizione { get; init; }
        public string? OrariUfficio { get; init; }
        public string? OrariConsegna { get; init; }
        public string? Provincia { get; init; }
        public string? Regione { get; init; }
        public short? Zoom { get; init; }
        public string? FleetNonVendibile { get; init; }
    }
}
