using Itera.BookingService.Infrastructure.Vehicle;
using Xunit;

namespace Itera.BookingService.Api.Tests.Integration.Vehicle;

/// <summary>
/// Test di integrazione su SQL Server reale (Testcontainers).
/// Verificano la query LINQ in LegacyVehicleQueryService: LEFT JOIN,
/// filtri CSV, MezzoSpeciale in-memory, ordinamento.
/// </summary>
[Collection(nameof(VehicleDatabaseCollection))]
public sealed class GetVehicleQueryServiceTests(VehicleDatabaseFixture db)
{
    // ------------------------------------------------------------------
    // 1. Nessun filtro — entrambi i modelli tornano
    // ------------------------------------------------------------------

    [Fact]
    public async Task GetMezzi_NoFilters_Returns_All_Visible_Models()
    {
        var svc = new LegacyVehicleQueryService(db.DbContext);

        var result = await svc.GetMezziAsync(null, null, null, null, CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    // ------------------------------------------------------------------
    // 2. LEFT JOIN — il modello senza gruppo non deve essere escluso
    // ------------------------------------------------------------------

    [Fact]
    public async Task GetMezzi_NoFilters_Includes_Model_Without_Gruppo()
    {
        var svc = new LegacyVehicleQueryService(db.DbContext);

        var result = await svc.GetMezziAsync(null, null, null, null, CancellationToken.None);

        // ModelloMezzoID=2 (MID) non ha righe in ModelloMezzoGruppo:
        // se il LEFT JOIN fosse diventato INNER JOIN, questo modello mancherebbe.
        Assert.Contains(result, r => r.ModelloMezzoID == VehicleDatabaseFixture.ModelloSenzaGruppo);
    }

    // ------------------------------------------------------------------
    // 3. Filtro FleetMulti CSV
    // ------------------------------------------------------------------

    [Fact]
    public async Task GetMezzi_FleetMulti_A_Returns_Only_Eco()
    {
        var svc = new LegacyVehicleQueryService(db.DbContext);

        var result = await svc.GetMezziAsync("A", null, null, null, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(VehicleDatabaseFixture.SegmentoEco, result[0].CodiceSegmento);
    }

    // ------------------------------------------------------------------
    // 4. Filtro MezzoSpeciale = true
    // ------------------------------------------------------------------

    [Fact]
    public async Task GetMezzi_MezzoSpeciale_True_Returns_Only_Mid()
    {
        var svc = new LegacyVehicleQueryService(db.DbContext);

        var result = await svc.GetMezziAsync(null, null, true, null, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(VehicleDatabaseFixture.SegmentoMid, result[0].CodiceSegmento);
    }

    // ------------------------------------------------------------------
    // 5. Filtro MezzoSpeciale = false
    // ------------------------------------------------------------------

    [Fact]
    public async Task GetMezzi_MezzoSpeciale_False_Returns_Only_Eco()
    {
        var svc = new LegacyVehicleQueryService(db.DbContext);

        var result = await svc.GetMezziAsync(null, null, false, null, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(VehicleDatabaseFixture.SegmentoEco, result[0].CodiceSegmento);
    }

    // ------------------------------------------------------------------
    // 6. Filtro GruppoID
    // ------------------------------------------------------------------

    [Fact]
    public async Task GetMezzi_GruppoID_Returns_Only_Associated_Model()
    {
        var svc = new LegacyVehicleQueryService(db.DbContext);

        var result = await svc.GetMezziAsync(null, null, null, VehicleDatabaseFixture.GruppoId, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(VehicleDatabaseFixture.ModelloConGruppo, result[0].ModelloMezzoID);
    }

    // ------------------------------------------------------------------
    // 7. Ordinamento ASC per SegmentoModello.Ordinamento
    //    MID ha Ordinamento=10, ECO ha Ordinamento=20 -> MID prima
    // ------------------------------------------------------------------

    [Fact]
    public async Task GetMezzi_Results_Are_Ordered_By_Ordinamento_Asc()
    {
        var svc = new LegacyVehicleQueryService(db.DbContext);

        var result = await svc.GetMezziAsync(null, null, null, null, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(VehicleDatabaseFixture.SegmentoMid, result[0].CodiceSegmento); // Ordinamento=10
        Assert.Equal(VehicleDatabaseFixture.SegmentoEco, result[1].CodiceSegmento); // Ordinamento=20
    }
}
