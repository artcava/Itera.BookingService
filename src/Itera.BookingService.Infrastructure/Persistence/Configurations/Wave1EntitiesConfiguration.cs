using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class WsUserGruppoConfiguration : IEntityTypeConfiguration<WsUserGruppo>
{
    public void Configure(EntityTypeBuilder<WsUserGruppo> builder)
    {
        builder.ToTable("WsUserGruppo", "dbo");
        builder.HasKey(x => x.WsUserGruppoID);
    }
}

public class WsUserListinoConfiguration : IEntityTypeConfiguration<WsUserListino>
{
    public void Configure(EntityTypeBuilder<WsUserListino> builder)
    {
        builder.ToTable("WsUserListino", "dbo");
        builder.HasKey(x => x.WsUserListinoID);
    }
}

public class FilialeConfiguration : IEntityTypeConfiguration<Filiale>
{
    public void Configure(EntityTypeBuilder<Filiale> builder)
    {
        builder.ToTable("Filiale", "dbo");
        builder.HasKey(x => x.FilialeID);
    }
}

public class FilialeTestoConfiguration : IEntityTypeConfiguration<FilialeTesto>
{
    public void Configure(EntityTypeBuilder<FilialeTesto> builder)
    {
        builder.ToTable("FilialeTesto", "dbo");
        builder.HasKey(x => x.FilialeID);
    }
}

public class FranchiseConfiguration : IEntityTypeConfiguration<Franchise>
{
    public void Configure(EntityTypeBuilder<Franchise> builder)
    {
        builder.ToTable("Franchise", "dbo");
        builder.HasKey(x => x.FranchiseID);
    }
}

public class FilialeAreaConfiguration : IEntityTypeConfiguration<FilialeArea>
{
    public void Configure(EntityTypeBuilder<FilialeArea> builder)
    {
        builder.ToTable("FilialeArea", "dbo");
        builder.HasKey(x => x.FilialeAreaID);
    }
}

public class FilialeClassificazioneConfiguration : IEntityTypeConfiguration<FilialeClassificazione>
{
    public void Configure(EntityTypeBuilder<FilialeClassificazione> builder)
    {
        builder.ToTable("FilialeClassificazione", "dbo");
        builder.HasKey(x => x.FilialeClassificazioneID);
    }
}

public class ProvinciaConfiguration : IEntityTypeConfiguration<Provincia>
{
    public void Configure(EntityTypeBuilder<Provincia> builder)
    {
        builder.ToTable("Provincia", "dbo");
        builder.HasKey(x => x.SiglaAutomobilistica);
    }
}

public class RegioneConfiguration : IEntityTypeConfiguration<Regione>
{
    public void Configure(EntityTypeBuilder<Regione> builder)
    {
        builder.ToTable("Regione", "dbo");
        builder.HasKey(x => x.CodiceRegione);
    }
}

public class FilialeBrandConfiguration : IEntityTypeConfiguration<FilialeBrand>
{
    public void Configure(EntityTypeBuilder<FilialeBrand> builder)
    {
        builder.ToTable("FilialeBrand", "dbo");
        builder.HasKey(x => x.FilialeBrandID);
    }
}

public class TestoConfiguration : IEntityTypeConfiguration<Testo>
{
    public void Configure(EntityTypeBuilder<Testo> builder)
    {
        builder.ToTable("Testo", "dbo");
        builder.HasKey(x => x.TestoID);
    }
}

public class FilialeChiusuraExtraConfiguration : IEntityTypeConfiguration<FilialeChiusuraExtra>
{
    public void Configure(EntityTypeBuilder<FilialeChiusuraExtra> builder)
    {
        builder.ToTable("FilialeChiusuraExtra", "dbo");
        builder.HasKey(x => x.FilialeChiusuraExtraID);
    }
}

public class FilialeFasciaOrarioConfiguration : IEntityTypeConfiguration<FilialeFasciaOrario>
{
    public void Configure(EntityTypeBuilder<FilialeFasciaOrario> builder)
    {
        builder.ToTable("FilialeFasciaOrario", "dbo");
        builder.HasKey(x => x.FilialeFasciaOrarioID);
    }
}

public class FilialeOrarioOperativoConfiguration : IEntityTypeConfiguration<FilialeOrarioOperativo>
{
    public void Configure(EntityTypeBuilder<FilialeOrarioOperativo> builder)
    {
        builder.ToTable("FilialeOrarioOperativo", "dbo");
        builder.HasKey(x => x.FilialeOrarioOperativoID);
    }
}

public class FilialeOrarioOperativoVariazioneConfiguration : IEntityTypeConfiguration<FilialeOrarioOperativoVariazione>
{
    public void Configure(EntityTypeBuilder<FilialeOrarioOperativoVariazione> builder)
    {
        builder.ToTable("FilialeOrarioOperativoVariazione", "dbo");
        builder.HasKey(x => x.FilialeOrarioOperativoVariazioneID);
    }
}

public class FilialeRiposoSettimanaleConfiguration : IEntityTypeConfiguration<FilialeRiposoSettimanale>
{
    public void Configure(EntityTypeBuilder<FilialeRiposoSettimanale> builder)
    {
        builder.ToTable("FilialeRiposoSettimanale", "dbo");
        builder.HasKey(x => x.FilialeRiposoSettimanaleID);
    }
}

public class FilialeValConfiguration : IEntityTypeConfiguration<FilialeVal>
{
    public void Configure(EntityTypeBuilder<FilialeVal> builder)
    {
        builder.ToTable("FilialeVAL", "dbo");
        builder.HasKey(x => x.FilialeVALID);
    }
}

// PK stringa varchar(20) — ValueGeneratedNever obbligatorio
public class MezzoConfiguration : IEntityTypeConfiguration<Mezzo>
{
    public void Configure(EntityTypeBuilder<Mezzo> builder)
    {
        builder.ToTable("Mezzo", "dbo");
        builder.HasKey(x => x.CodiceMezzo);
        builder.Property(x => x.CodiceMezzo).HasMaxLength(20).ValueGeneratedNever();
        builder.Property(x => x.Targa).HasMaxLength(100);
        builder.Property(x => x.Telaio).HasMaxLength(100);
        builder.Property(x => x.CodiceAutoradio).HasMaxLength(20);
        builder.Property(x => x.KeyCode).HasMaxLength(20);
        builder.Property(x => x.ColoreInterno).HasMaxLength(50);
        builder.Property(x => x.ColoreEsterno).HasMaxLength(50);
        builder.Property(x => x.CodiceMezzoFinale).HasMaxLength(100);
        builder.Property(x => x.CodiceMezzoVisualizzato).HasMaxLength(100);
        builder.Property(x => x.Note).HasColumnType("text");
        builder.Property(x => x.SubCodice).HasMaxLength(100);
        builder.Property(x => x.ImportoVAL).HasColumnType("money");
    }
}

// PK stringa varchar(3) — ValueGeneratedNever obbligatorio
public class SegmentoModelloConfiguration : IEntityTypeConfiguration<SegmentoModello>
{
    public void Configure(EntityTypeBuilder<SegmentoModello> builder)
    {
        builder.ToTable("SegmentoModello", "dbo");
        builder.HasKey(x => x.CodiceSegmento);
        builder.Property(x => x.CodiceSegmento).HasMaxLength(3).ValueGeneratedNever();
        builder.Property(x => x.CodiceCategoria).HasMaxLength(5);
        builder.Property(x => x.Descrizione).HasMaxLength(50);
        builder.Property(x => x.FleetID).HasMaxLength(1);
        builder.Property(x => x.ImportoVAL).HasColumnType("money");
    }
}

public class PreventivoConfiguration : IEntityTypeConfiguration<Preventivo>
{
    public void Configure(EntityTypeBuilder<Preventivo> builder)
    {
        builder.ToTable("Preventivo", "dbo");
        builder.HasKey(x => x.PreventivoID);
    }
}

public class WsTokenPreventivoConfiguration : IEntityTypeConfiguration<WsTokenPreventivo>
{
    public void Configure(EntityTypeBuilder<WsTokenPreventivo> builder)
    {
        builder.ToTable("WsTokenPreventivo", "dbo");
        builder.HasKey(x => x.WsTokenPreventivoID);
    }
}

public class ListinoConfiguration : IEntityTypeConfiguration<Listino>
{
    public void Configure(EntityTypeBuilder<Listino> builder)
    {
        builder.ToTable("Listino", "dbo");
        builder.HasKey(x => x.ListinoID);
    }
}

public class ListinoBrandConfiguration : IEntityTypeConfiguration<ListinoBrand>
{
    public void Configure(EntityTypeBuilder<ListinoBrand> builder)
    {
        builder.ToTable("ListinoBrand", "dbo");
        builder.HasKey(x => x.ListinoBrandID);
    }
}

public class ListinoGiorniConfiguration : IEntityTypeConfiguration<ListinoGiorni>
{
    public void Configure(EntityTypeBuilder<ListinoGiorni> builder)
    {
        builder.ToTable("ListinoGiorni", "dbo");
        builder.HasKey(x => x.ListinoGiorniID);
    }
}

public class ListinoKmConfiguration : IEntityTypeConfiguration<ListinoKm>
{
    public void Configure(EntityTypeBuilder<ListinoKm> builder)
    {
        builder.ToTable("ListinoKm", "dbo");
        builder.HasKey(x => x.ListinoKmID);
    }
}

public class KmConfiguration : IEntityTypeConfiguration<Km>
{
    public void Configure(EntityTypeBuilder<Km> builder)
    {
        builder.ToTable("Km", "dbo");
        builder.HasKey(x => x.KmID);
    }
}

public class ListinoValoriConfiguration : IEntityTypeConfiguration<ListinoValori>
{
    public void Configure(EntityTypeBuilder<ListinoValori> builder)
    {
        builder.ToTable("ListinoValori", "dbo");
        builder.HasKey(x => x.ListinoValoriID);
    }
}

public class ListinoFranchigiaConfiguration : IEntityTypeConfiguration<ListinoFranchigia>
{
    public void Configure(EntityTypeBuilder<ListinoFranchigia> builder)
    {
        builder.ToTable("ListinoFranchigia", "dbo");
        builder.HasKey(x => x.ListinoFranchigiaID);
    }
}

public class TipologiaFranchigiaConfiguration : IEntityTypeConfiguration<TipologiaFranchigia>
{
    public void Configure(EntityTypeBuilder<TipologiaFranchigia> builder)
    {
        builder.ToTable("TipologiaFranchigia", "dbo");
        builder.HasKey(x => x.TipologiaFranchigiaID);
    }
}

public class AccordoCommercialeListinoConfiguration : IEntityTypeConfiguration<AccordoCommercialeListino>
{
    public void Configure(EntityTypeBuilder<AccordoCommercialeListino> builder)
    {
        builder.ToTable("AccordoCommercialeListino", "dbo");
        builder.HasKey(x => x.AccordoCommercialeListinoID);
    }
}

public class RegolaDiVenditaListinoConfiguration : IEntityTypeConfiguration<RegolaDiVenditaListino>
{
    public void Configure(EntityTypeBuilder<RegolaDiVenditaListino> builder)
    {
        builder.ToTable("RegolaDiVenditaListino", "dbo");
        builder.HasKey(x => x.RegolaDiVenditaListinoID);
    }
}
