using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations.Estimate;

public class IvaConfiguration : IEntityTypeConfiguration<Iva>
{
    public void Configure(EntityTypeBuilder<Iva> builder)
    {
        builder.ToTable("Iva");

        builder.HasKey(x => x.IvaId);

        builder.Property(x => x.IvaId)
            .HasColumnName("IvaID");

        builder.Property(x => x.Percentuale)
            .HasPrecision(5, 2);

        builder.Property(x => x.ValidaDal)
            .HasColumnType("datetime");

        builder.Property(x => x.ValidaAl)
            .HasColumnType("datetime");

        builder.Property(x => x.Descrizione)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(x => x.DescrizioneSuFattura)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(x => x.NotaAggiuntivaSuFatturaIta)
            .HasColumnName("NotaAggiuntivaSuFatturaITA")
            .HasMaxLength(500)
            .IsUnicode(true);

        builder.Property(x => x.NotaAggiuntivaSuFatturaEng)
            .HasColumnName("NotaAggiuntivaSuFatturaENG")
            .HasMaxLength(500)
            .IsUnicode(true);

        builder.Property(x => x.IvaIdSostituzione365Plus)
            .HasColumnName("IvaIDSostituzione365Plus");

        builder.Property(x => x.Sistema)
            .HasDefaultValue(true);

        builder.Property(x => x.SplitPayment)
            .HasDefaultValue(false);

        builder.HasOne(x => x.IvaSostituzione365Plus)
            .WithMany(x => x.IvaSostituzioni365Plus)
            .HasForeignKey(x => x.IvaIdSostituzione365Plus)
            .HasConstraintName("FK_Iva_Iva365Plus");
    }
}