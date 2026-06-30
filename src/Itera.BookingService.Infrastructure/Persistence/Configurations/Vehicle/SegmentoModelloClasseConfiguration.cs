using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class SegmentoModelloClasseConfiguration : IEntityTypeConfiguration<SegmentoModelloClasse>
{
    public void Configure(EntityTypeBuilder<SegmentoModelloClasse> builder)
    {
        builder.ToTable("SegmentoModelloClasse", "dbo");
        builder.HasKey(x => x.SegmentoModelloClasseID);
        builder.Property(x => x.SegmentoModelloClasseID).ValueGeneratedNever();
        builder.Property(x => x.Descrizione).HasMaxLength(50);
    }
}
