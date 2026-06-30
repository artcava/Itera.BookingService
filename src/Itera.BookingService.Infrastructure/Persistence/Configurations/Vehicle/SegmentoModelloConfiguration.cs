using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

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
