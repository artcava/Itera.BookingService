using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class RipartizioneFatturatoConfiguration : IEntityTypeConfiguration<RipartizioneFatturato>
{
    public void Configure(EntityTypeBuilder<RipartizioneFatturato> builder)
    {
        builder.ToTable("RipartizioneFatturato", "dbo");

        builder.HasKey(x => x.RipartizioneFatturatoID);

        builder.Property(x => x.RipartizioneFatturatoID)
            .HasColumnName("RipartizioneFatturatoID")
            .HasMaxLength(1)
            .IsFixedLength()
            .IsRequired();

        builder.Property(x => x.Descrizione)
            .HasColumnName("Descrizione")
            .HasMaxLength(50);

        builder.HasMany(x => x.TipologieVoceFattura)
            .WithOne(x => x.RipartizioneFatturato)
            .HasForeignKey(x => x.RipartizioneFatturatoID);
    }
}