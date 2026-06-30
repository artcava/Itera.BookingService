using Itera.BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace Itera.BookingService.Api.Tests.Integration.Vehicle;

/// <summary>
/// Avvia un container SQL Server reale, applica lo schema Vehicle minimo
/// e inserisce un seed deterministico condiviso da tutti i test della suite.
/// Il container viene creato una sola volta per collection (IAsyncLifetime su ICollectionFixture).
/// </summary>
public sealed class VehicleDatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
        .Build();

    public LegacyDbContext DbContext { get; private set; } = null!;

    // ------------------------------------------------------------------
    // Seed constants — valori noti usati negli Assert dei test
    // ------------------------------------------------------------------

    /// <summary>Segmento con FleetID "A", Ordinamento 20, NON speciale.</summary>
    public const string SegmentoEco  = "ECO";
    /// <summary>Segmento con FleetID "B", Ordinamento 10, IS speciale.</summary>
    public const string SegmentoMid  = "MID";
    public const int    GruppoId     = 1;
    /// <summary>ModelloMezzoID del modello associato al GruppoId.</summary>
    public const int    ModelloConGruppo    = 1;   // ECO
    /// <summary>ModelloMezzoID del modello senza nessun gruppo.</summary>
    public const int    ModelloSenzaGruppo  = 2;   // MID

    // ------------------------------------------------------------------

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var cs = _container.GetConnectionString();
        await ApplySchemaAsync(cs);
        DbContext = BuildContext(cs);
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _container.DisposeAsync();
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static LegacyDbContext BuildContext(string connectionString)
    {
        var opts = new DbContextOptionsBuilder<LegacyDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        return new LegacyDbContext(opts);
    }

    private static async Task ApplySchemaAsync(string connectionString)
    {
        var opts = new DbContextOptionsBuilder<LegacyDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        await using var ctx = new LegacyDbContext(opts);
        await ctx.Database.ExecuteSqlRawAsync(SchemaAndSeedSql);
    }

    // ------------------------------------------------------------------
    // Schema DDL + seed SQL
    // Tabelle nella stessa sequenza dei FK per evitare violazioni.
    // IDENTITY inserita via SET IDENTITY_INSERT dove necessario.
    // ------------------------------------------------------------------

    private const string SchemaAndSeedSql = """
        -- SegmentoModelloClasse
        CREATE TABLE [dbo].[SegmentoModelloClasse] (
            [SegmentoModelloClasseID] INT          NOT NULL,
            [Descrizione]            VARCHAR(50)  NULL,
            CONSTRAINT [PK_SegmentoModelloClasse] PRIMARY KEY ([SegmentoModelloClasseID])
        );

        -- AlimentazioneModello
        CREATE TABLE [dbo].[AlimentazioneModello] (
            [AlimentazioneModelloID] SMALLINT     NOT NULL IDENTITY(1,1),
            [Descrizione]           VARCHAR(30)  NULL,
            CONSTRAINT [PK_AlimentazioneModello] PRIMARY KEY ([AlimentazioneModelloID])
        );

        -- Marca
        CREATE TABLE [dbo].[Marca] (
            [MarcaID]     SMALLINT     NOT NULL IDENTITY(1,1),
            [Descrizione] VARCHAR(30)  NULL,
            CONSTRAINT [PK_Marca] PRIMARY KEY ([MarcaID])
        );

        -- SegmentoModello
        CREATE TABLE [dbo].[SegmentoModello] (
            [CodiceSegmento]          VARCHAR(3)   NOT NULL,
            [CodiceCategoria]         VARCHAR(5)   NOT NULL DEFAULT '',
            [Descrizione]             VARCHAR(50)  NOT NULL DEFAULT '',
            [Ordinamento]             TINYINT      NOT NULL DEFAULT 0,
            [ModelloMezzoIDPdfOfferta] INT          NULL,
            [Stato]                   TINYINT      NULL,
            [ModelloMezzoIDErs]       INT          NULL,
            [FleetID]                 CHAR(1)      NULL,
            [SegmentoModelloClasseID] INT          NULL,
            [IndexPricing]            SMALLINT     NULL,
            [ListinoID]               INT          NULL,
            [ImportoVAL]              MONEY        NULL,
            [DataInserimento]         DATETIME     NULL,
            [DataUltimaModifica]      DATETIME     NULL,
            CONSTRAINT [PK_SegmentoModello] PRIMARY KEY ([CodiceSegmento])
        );

        -- ModelloMezzo
        CREATE TABLE [dbo].[ModelloMezzo] (
            [ModelloMezzoID]              INT          NOT NULL IDENTITY(1,1),
            [CodiceSegmento]              VARCHAR(3)   NULL,
            [CodiceCategoria]             VARCHAR(5)   NULL,
            [MarcaID]                     SMALLINT     NOT NULL,
            [AlimentazioneModelloID]      SMALLINT     NOT NULL,
            [Serbatoio]                   SMALLINT     NULL,
            [Descrizione]                 VARCHAR(50)  NOT NULL,
            [NomeImmagine]                VARCHAR(100) NULL,
            [Passo]                       FLOAT        NULL,
            [LunghezzaEsterna]            FLOAT        NULL,
            [AltezzaEsterna]              FLOAT        NULL,
            [LarghezzaEsterna]            FLOAT        NULL,
            [Peso]                        SMALLINT     NULL,
            [LarghezzaPassaruote]         FLOAT        NULL,
            [LunghezzaInterna]            FLOAT        NULL,
            [AltezzaInterna]              FLOAT        NULL,
            [LarghezzaInterna]            FLOAT        NULL,
            [Portata]                     SMALLINT     NULL,
            [VolumeCarico]               SMALLINT     NULL,
            [NumeroPallets]               SMALLINT     NULL,
            [AltezzaCarico]               FLOAT        NULL,
            [MisurePortaPosteriore]       FLOAT        NULL,
            [MisurePortaLaterale]         FLOAT        NULL,
            [Cilindrata]                  SMALLINT     NULL,
            [Euro]                        CHAR(2)      NULL,
            [Km]                          INT          NULL,
            [CV]                          SMALLINT     NULL,
            [CapacitaSerbatoio]           SMALLINT     NULL,
            [NumeroPorte]                 TINYINT      NULL,
            [NumeroPosti]                 TINYINT      NULL,
            [Autoradio]                   BIT          NULL,
            [AriaCondizionata]            BIT          NULL,
            [Airbag]                      BIT          NULL,
            [Abs]                         BIT          NULL,
            [Eps]                         BIT          NULL,
            [MisuraGomme]                 VARCHAR(50)  NULL,
            [RuotaScorta]                 BIT          NULL,
            [KitGonfiaggio]               BIT          NULL,
            [DataModifica]                DATETIME     NOT NULL DEFAULT GETDATE(),
            [KmTagliandoFreni]            INT          NULL,
            [SogliaKmTagliandoFreni]      INT          NULL,
            [KmTagliandoOlio]             INT          NULL,
            [SogliaKmTagliandoOlio]       INT          NULL,
            [Ruotino]                     BIT          NULL,
            [NomeFileSchedaTecnica]       VARCHAR(150) NULL,
            [VisibilitaSito]              BIT          NULL,
            [ACRISSCode]                  VARCHAR(100) NULL,
            [NumeroPostiCarrozzina]       TINYINT      NULL,
            [NumeroPostiMobility]         TINYINT      NULL,
            [PedanaSollevatriceDoppioBraccio] BIT      NOT NULL DEFAULT 0,
            [DescrizioneMobilitySitoWeb_ITA]  NVARCHAR(MAX) NULL,
            [DescrizioneMobilitySitoWeb_ENG]  NVARCHAR(MAX) NULL,
            CONSTRAINT [PK_ModelloMezzo] PRIMARY KEY ([ModelloMezzoID])
        );

        -- SegmentoModelloMezzoSpeciale
        CREATE TABLE [dbo].[SegmentoModelloMezzoSpeciale] (
            [SegmentoModelloMezzoSpecialeID] INT NOT NULL IDENTITY(1,1),
            [CodiceSegmento]                 VARCHAR(3) NOT NULL,
            CONSTRAINT [PK_SegmentoModelloMezzoSpeciale] PRIMARY KEY ([SegmentoModelloMezzoSpecialeID])
        );

        -- ModelloMezzoGruppo
        CREATE TABLE [dbo].[ModelloMezzoGruppo] (
            [ModelloMezzoGruppoID] INT NOT NULL IDENTITY(1,1),
            [GruppoID]             INT NOT NULL,
            [ModelloMezzoID]       INT NOT NULL,
            CONSTRAINT [PK_ModelloMezzoGruppo] PRIMARY KEY ([ModelloMezzoGruppoID])
        );

        -- ================================================================
        -- SEED
        -- ================================================================

        -- lookup tables
        SET IDENTITY_INSERT [dbo].[SegmentoModelloClasse] ON;
        INSERT INTO [dbo].[SegmentoModelloClasse] ([SegmentoModelloClasseID],[Descrizione])
        VALUES (1,'Utilitaria'),(2,'Berlina');
        SET IDENTITY_INSERT [dbo].[SegmentoModelloClasse] OFF;

        INSERT INTO [dbo].[AlimentazioneModello] ([Descrizione]) VALUES ('Benzina'),('Diesel');
        INSERT INTO [dbo].[Marca] ([Descrizione]) VALUES ('Fiat'),('Volkswagen');

        -- ECO: FleetID='A', Ordinamento=20, SegmentoModelloClasseID=1
        INSERT INTO [dbo].[SegmentoModello]
            ([CodiceSegmento],[CodiceCategoria],[Descrizione],[Ordinamento],[FleetID],[SegmentoModelloClasseID])
        VALUES
            ('ECO','','Economy',20,'A',1),
            ('MID','','Intermediate',10,'B',2);

        -- ModelloMezzo: 2 righe, IDENTITY parte da 1
        -- ModelloMezzoID=1 -> ECO (MarcaID=1/Fiat, Alim=1/Benzina)
        -- ModelloMezzoID=2 -> MID (MarcaID=2/VW,  Alim=2/Diesel) VisibilitaSito=1
        INSERT INTO [dbo].[ModelloMezzo]
            ([CodiceSegmento],[MarcaID],[AlimentazioneModelloID],[Descrizione],[VisibilitaSito],[DataModifica])
        VALUES
            ('ECO',1,1,'Panda',NULL,GETDATE()),
            ('MID',2,2,'Golf', 1,  GETDATE());

        -- Solo MID e' speciale
        INSERT INTO [dbo].[SegmentoModelloMezzoSpeciale] ([CodiceSegmento]) VALUES ('MID');

        -- Solo il modello ECO (ID=1) appartiene al gruppo 1
        INSERT INTO [dbo].[ModelloMezzoGruppo] ([GruppoID],[ModelloMezzoID]) VALUES (1,1);
        """;
}
