using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations.Estimate;

public class TariffarioConfiguration : IEntityTypeConfiguration<Tariffario>
{
    public void Configure(EntityTypeBuilder<Tariffario> builder)
    {
        builder.ToTable("Tariffario", "pricing");

        builder.HasKey(x => x.TariffarioID);

        builder.Property(x => x.TariffarioID)
            .HasColumnName("TariffarioID")
            .UseIdentityColumn();

        builder.Property(x => x.Descrizione)
            .HasColumnName("Descrizione")
            .HasMaxLength(100);

        builder.Property(x => x.BrandID)
            .HasColumnName("BrandID");

        builder.Property(x => x.Codice)
            .HasColumnName("Codice")
            .HasMaxLength(20);

        builder.HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandID);

        builder.HasMany(x => x.TariffeRdv)
            .WithOne(x => x.Tariffario)
            .HasForeignKey(x => x.TariffarioID);

        builder.HasIndex(x => x.Codice)
            .IsUnique()
            .HasDatabaseName("UNIQUE_CODICETARIFFARIO");

        builder.HasIndex(x => x.Descrizione)
            .IsUnique()
            .HasDatabaseName("UNIQUE_DESCRIZIONETARIFFARIO");
    }
}