using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class MezzoConfiguration : IEntityTypeConfiguration<Mezzo>
{
    public void Configure(EntityTypeBuilder<Mezzo> builder)
    {
        builder.ToTable("Mezzo", "dbo");
        builder.HasKey(x => x.CodiceMezzo);
        builder.Property(x => x.CodiceMezzo).HasMaxLength(20).ValueGeneratedNever();
        builder.Property(x => x.Targa).HasMaxLength(100);
        builder.Property(x => x.Telaio).HasMaxLength(100);
        builder.Property(x => x.CodiceAutoradio).HasMaxLength(20);
        builder.Property(x => x.KeyCode).HasMaxLength(20);
        builder.Property(x => x.ColoreInterno).HasMaxLength(50);
        builder.Property(x => x.ColoreEsterno).HasMaxLength(50);
        builder.Property(x => x.CodiceMezzoFinale).HasMaxLength(100);
        builder.Property(x => x.CodiceMezzoVisualizzato).HasMaxLength(100);
        builder.Property(x => x.Note).HasColumnType("text");
        builder.Property(x => x.SubCodice).HasMaxLength(100);
    }
}
