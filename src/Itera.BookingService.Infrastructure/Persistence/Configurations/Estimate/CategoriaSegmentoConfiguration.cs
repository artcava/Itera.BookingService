using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class CategoriaSegmentoConfiguration : IEntityTypeConfiguration<CategoriaSegmento>
{
    public void Configure(EntityTypeBuilder<CategoriaSegmento> builder)
    {
        builder.ToTable("CategoriaSegmento", "dbo");

        builder.HasKey(x => x.CodiceCategoria);

        builder.Property(x => x.CodiceCategoria)
            .HasColumnName("CodiceCategoria")
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(x => x.Descrizione)
            .HasColumnName("Descrizione")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.VoceContabile)
            .HasColumnName("VoceContabile")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.DescrizioneVoceContabile)
            .HasColumnName("DescrizioneVoceContabile")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.NomeFileImmagine)
            .HasColumnName("NomeFileImmagine")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasColumnName("Tipo")
            .HasMaxLength(1)
            .IsFixedLength()
            .IsRequired();

        builder.Property(x => x.DescrizioneSAP)
            .HasColumnName("DescrizioneSAP")
            .HasMaxLength(15);

        builder.HasMany(x => x.TipologieVoceFattura)
            .WithOne(x => x.CategoriaSegmento)
            .HasForeignKey(x => x.CodiceCategoriaSegmento);
    }
}