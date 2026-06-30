using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

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
