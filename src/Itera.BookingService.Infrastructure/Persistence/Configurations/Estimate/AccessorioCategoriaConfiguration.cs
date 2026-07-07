using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class AccessorioCategoriaConfiguration : IEntityTypeConfiguration<AccessorioCategoria>
{
    public void Configure(EntityTypeBuilder<AccessorioCategoria> builder)
    {
        builder.ToTable("AccessorioCategoria", "dbo");

        builder.HasKey(x => x.AccessorioCategoriaID);

        builder.Property(x => x.AccessorioCategoriaID)
            .HasColumnName("AccessorioCategoriaID");

        builder.Property(x => x.Descrizione)
            .HasColumnName("Descrizione")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Codice)
            .HasColumnName("Codice")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.BrandID)
            .HasColumnName("BrandID");

        builder.Property(x => x.StatoID)
            .HasColumnName("StatoID")
            .IsRequired();

        builder.HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandID);

        builder.HasMany(x => x.AccessorioTipologie)
            .WithOne(x => x.AccessorioCategoria)
            .HasForeignKey(x => x.AccessorioCategoriaID);
    }
}