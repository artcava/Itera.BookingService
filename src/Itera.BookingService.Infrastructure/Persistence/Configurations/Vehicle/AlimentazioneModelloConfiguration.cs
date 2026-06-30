using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class AlimentazioneModelloConfiguration : IEntityTypeConfiguration<AlimentazioneModello>
{
    public void Configure(EntityTypeBuilder<AlimentazioneModello> builder)
    {
        builder.ToTable("AlimentazioneModello", "dbo");
        builder.HasKey(x => x.AlimentazioneModelloID);
        builder.Property(x => x.Descrizione).HasMaxLength(50).IsRequired();
    }
}
