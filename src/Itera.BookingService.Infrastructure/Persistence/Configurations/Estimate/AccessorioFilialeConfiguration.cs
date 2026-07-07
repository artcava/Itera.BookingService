using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;
public class AccessorioFilialeConfiguration : IEntityTypeConfiguration<AccessorioFiliale>
{
    public void Configure(EntityTypeBuilder<AccessorioFiliale> builder)
    {
        builder.ToTable("AccessorioFiliale", "dbo");

        builder.HasKey(x => x.AccessorioFilialeID);

        builder.Property(x => x.AccessorioFilialeID)
            .HasColumnName("AccessorioFilialeID")
            .UseIdentityColumn();

        builder.Property(x => x.AccessorioTipologiaID)
            .HasColumnName("AccessorioTipologiaID")
            .IsRequired();

        builder.Property(x => x.FilialeID)
            .HasColumnName("FilialeID")
            .IsRequired();

        builder.Property(x => x.Note)
            .HasColumnName("Note")
            .HasMaxLength(50);

        builder.HasOne(x => x.AccessorioTipologia)
            .WithMany(x => x.AccessorioFiliali)
            .HasForeignKey(x => x.AccessorioTipologiaID);

        builder.HasOne(x => x.Filiale)
            .WithMany()
            .HasForeignKey(x => x.FilialeID);
    }
}