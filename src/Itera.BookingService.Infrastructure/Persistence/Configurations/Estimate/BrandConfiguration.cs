using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brand", "dbo");

        builder.HasKey(x => x.BrandID);

        builder.Property(x => x.BrandID)
            .HasColumnName("BrandID");

        builder.Property(x => x.CodiceBrand)
            .HasColumnName("CodiceBrand")
            .HasMaxLength(3)
            .IsFixedLength()
            .IsRequired();

        builder.Property(x => x.Descrizione)
            .HasColumnName("Descrizione")
            .HasMaxLength(250);

        builder.Property(x => x.DataInserimento)
            .HasColumnName("DataInserimento");

        builder.Property(x => x.DataUltimaModifica)
            .HasColumnName("DataUltimaModifica");

        builder.Property(x => x.DescrizioneSAP)
            .HasColumnName("DescrizioneSAP")
            .HasMaxLength(15);

        builder.Property(x => x.DescrizioneMit)
            .HasColumnName("DescrizioneMit")
            .HasMaxLength(250);

        builder.Property(x => x.LogoImmagine)
            .HasColumnName("LogoImmagine")
            .HasMaxLength(100);

        builder.Property(x => x.EmailFrom)
            .HasColumnName("EmailFrom")
            .HasMaxLength(100);

        builder.Property(x => x.EmailFromAlias)
            .HasColumnName("EmailFromAlias")
            .HasMaxLength(100);

        builder.HasMany(x => x.AccessorioCategorie)
            .WithOne(x => x.Brand)
            .HasForeignKey(x => x.BrandID);

        builder.HasMany(x => x.AccessorioTipologie)
            .WithOne(x => x.Brand)
            .HasForeignKey(x => x.BrandID);
    }
}