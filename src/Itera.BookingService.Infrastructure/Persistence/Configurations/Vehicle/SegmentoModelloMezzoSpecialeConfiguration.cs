using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class SegmentoModelloMezzoSpecialeConfiguration : IEntityTypeConfiguration<SegmentoModelloMezzoSpeciale>
{
    public void Configure(EntityTypeBuilder<SegmentoModelloMezzoSpeciale> builder)
    {
        builder.ToTable("SegmentoModelloMezzoSpeciale", "dbo");
        builder.HasKey(x => x.SegmentoModelloMezzoSpecialeID);
        builder.Property(x => x.CodiceSegmento).HasMaxLength(3).IsRequired();
    }
}
