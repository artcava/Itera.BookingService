using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class AccessorioSegmentoConfiguration : IEntityTypeConfiguration<AccessorioSegmento>
{
    public void Configure(EntityTypeBuilder<AccessorioSegmento> builder)
    {
        builder.ToTable("AccessorioSegmento", "dbo");

        builder.HasKey(x => x.AccessorioSegmentoID);

        builder.Property(x => x.AccessorioSegmentoID)
            .HasColumnName("AccessorioSegmentoID")
            .UseIdentityColumn();

        builder.Property(x => x.AccessorioTipologiaID)
            .HasColumnName("AccessorioTipologiaID")
            .IsRequired();

        builder.Property(x => x.CodiceSegmento)
            .HasColumnName("CodiceSegmento")
            .HasMaxLength(3)
            .IsRequired();

        builder.HasOne(x => x.AccessorioTipologia)
            .WithMany(x => x.AccessorioSegmenti)
            .HasForeignKey(x => x.AccessorioTipologiaID);

        builder.HasOne(x => x.SegmentoModello)
            .WithMany()
            .HasForeignKey(x => x.CodiceSegmento);
    }
}