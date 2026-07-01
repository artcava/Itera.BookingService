using Itera.BookingService.Infrastructure.Persistence.Entities;
using Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class ListinoConfiguration : IEntityTypeConfiguration<Listino>
{
    public void Configure(EntityTypeBuilder<Listino> builder)
    {
        builder.ToTable("Listino", "dbo");
        builder.HasKey(x => x.ListinoId);
        builder.Property(x => x.ListinoId).HasColumnName("ListinoID");
        builder.Property(x => x.SempreAttivo).HasColumnName("SempreAttivo").IsRequired();
        builder.Property(x => x.InizioValidita).HasColumnName("InizioValidita");
        builder.Property(x => x.FineValidita).HasColumnName("FineValidita");

        builder.HasMany(x => x.ListinoGiorni)
               .WithOne(x => x.Listino)
               .HasForeignKey(x => x.ListinoId);

        builder.HasMany(x => x.ListinoFiliali)
               .WithOne()
               .HasForeignKey(x => x.ListinoId);
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

public class ListinoFilialeConfiguration : IEntityTypeConfiguration<ListinoFiliale>
{
    public void Configure(EntityTypeBuilder<ListinoFiliale> builder)
    {
        builder.ToTable("ListinoFiliale", "dbo");
        builder.HasKey(x => new { x.FilialeId, x.ListinoId });
        builder.Property(x => x.FilialeId).HasColumnName("FilialeID");
        builder.Property(x => x.ListinoId).HasColumnName("ListinoID");
    }
}

public class ListinoGiorniConfiguration : IEntityTypeConfiguration<ListinoGiorni>
{
    public void Configure(EntityTypeBuilder<ListinoGiorni> builder)
    {
        builder.ToTable("ListinoGiorni", "dbo");
        builder.HasKey(x => x.ListinoGiorniId);
        builder.Property(x => x.ListinoGiorniId).HasColumnName("ListinoGiorniID");
        builder.Property(x => x.ListinoId).HasColumnName("ListinoID");
        builder.Property(x => x.Codice).HasMaxLength(30).IsRequired();
        builder.Property(x => x.CodiceCategoria).HasMaxLength(5).IsRequired();
        builder.Property(x => x.Descrizione).HasMaxLength(50).IsRequired();
        builder.Property(x => x.FasciaStart).HasColumnName("FasciaStart").IsRequired();
        builder.Property(x => x.FasciaEnd).HasColumnName("FasciaEnd").IsRequired();
        builder.Property(x => x.IsVisible).HasColumnName("IsVisible");

        builder.HasMany(x => x.ListinoKm)
               .WithOne(x => x.ListinoGiorni)
               .HasForeignKey(x => x.ListinoGiorniId);
    }
}

public class ListinoKmConfiguration : IEntityTypeConfiguration<ListinoKm>
{
    public void Configure(EntityTypeBuilder<ListinoKm> builder)
    {
        builder.ToTable("ListinoKm", "dbo");
        builder.HasKey(x => x.ListinoKmId);
        builder.Property(x => x.ListinoKmId).HasColumnName("ListinoKmID").UseIdentityColumn();
        builder.Property(x => x.ListinoGiorniId).HasColumnName("ListinoGiorniID").IsRequired();
        builder.Property(x => x.Km).HasColumnName("Km").IsRequired();
        builder.Property(x => x.Ordinamento).HasColumnName("Ordinamento").IsRequired();
        builder.Property(x => x.IsVisible).HasColumnName("IsVisible");
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
