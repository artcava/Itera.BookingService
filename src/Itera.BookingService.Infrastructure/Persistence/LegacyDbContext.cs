using Microsoft.EntityFrameworkCore;

namespace Itera.BookingService.Infrastructure.Persistence;

public class LegacyDbContext(DbContextOptions<LegacyDbContext> options) : DbContext(options)
{
    // Security/API token
    public DbSet<WsUser> WsUsers => Set<WsUser>();
    public DbSet<WsToken> WsTokens => Set<WsToken>();
    public DbSet<WsUserGruppo> WsUserGruppi => Set<WsUserGruppo>();
    public DbSet<WsUserListino> WsUserListini => Set<WsUserListino>();

    // Branch
    public DbSet<Filiale> Filiali => Set<Filiale>();
    public DbSet<FilialeTesto> FilialeTesti => Set<FilialeTesto>();
    public DbSet<FilialeChiusuraExtra> FilialeChiusureExtra => Set<FilialeChiusuraExtra>();
    public DbSet<FilialeFasciaOrario> FilialeFasceOrario => Set<FilialeFasciaOrario>();
    public DbSet<FilialeOrarioOperativo> FilialeOrariOperativi => Set<FilialeOrarioOperativo>();
    public DbSet<FilialeOrarioOperativoVariazione> FilialeOrariOperativiVariazioni => Set<FilialeOrarioOperativoVariazione>();
    public DbSet<FilialeRiposoSettimanale> FilialeRiposiSettimanali => Set<FilialeRiposoSettimanale>();
    public DbSet<FilialeVal> FilialeValori => Set<FilialeVal>();
    public DbSet<Franchise> Franchises => Set<Franchise>();
    public DbSet<FilialeArea> FilialeAree => Set<FilialeArea>();
    public DbSet<FilialeClassificazione> FilialeClassificazioni => Set<FilialeClassificazione>();
    public DbSet<Provincia> Province => Set<Provincia>();
    public DbSet<Regione> Regioni => Set<Regione>();
    public DbSet<FilialeBrand> FilialeBrands => Set<FilialeBrand>();
    public DbSet<Testo> Testi => Set<Testo>();

    // Vehicle
    public DbSet<Mezzo> Mezzi => Set<Mezzo>();
    public DbSet<SegmentoModello> SegmentiModello => Set<SegmentoModello>();

    // Estimate
    public DbSet<Preventivo> Preventivi => Set<Preventivo>();
    public DbSet<WsTokenPreventivo> WsTokenPreventivi => Set<WsTokenPreventivo>();
    public DbSet<Listino> Listini => Set<Listino>();
    public DbSet<ListinoBrand> ListinoBrand => Set<ListinoBrand>();
    public DbSet<ListinoGiorni> ListinoGiorni => Set<ListinoGiorni>();
    public DbSet<ListinoKm> ListinoKm => Set<ListinoKm>();
    public DbSet<Km> Km => Set<Km>();
    public DbSet<ListinoValori> ListinoValori => Set<ListinoValori>();
    public DbSet<ListinoFranchigia> ListinoFranchigie => Set<ListinoFranchigia>();
    public DbSet<TipologiaFranchigia> TipologieFranchigia => Set<TipologiaFranchigia>();
    public DbSet<AccordoCommercialeListino> AccordiCommercialiListino => Set<AccordoCommercialeListino>();
    public DbSet<RegolaDiVenditaListino> RegoleDiVenditaListino => Set<RegolaDiVenditaListino>();

    // Correlated query/SP/view types (Wave 2 keyless)
    public DbSet<GetFilialeInfoWs2Result> GetFilialeInfoWs2Results => Set<GetFilialeInfoWs2Result>();
    public DbSet<GetFilialiFatturazioneClienteWsResult> GetFilialiFatturazioneClienteWsResults => Set<GetFilialiFatturazioneClienteWsResult>();
    public DbSet<GetMezziWsResult> GetMezziWsResults => Set<GetMezziWsResult>();
    public DbSet<GetListinoValoriResult> GetListinoValoriResults => Set<GetListinoValoriResult>();
    public DbSet<GetKmInfoMultiResult> GetKmInfoMultiResults => Set<GetKmInfoMultiResult>();
    public DbSet<VwListinoFranchigiaResult> VwListinoFranchigiaResults => Set<VwListinoFranchigiaResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LegacyDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
