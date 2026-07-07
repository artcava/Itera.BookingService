using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class TipologiaVoceFatturaConfiguration : IEntityTypeConfiguration<TipologiaVoceFattura>
{
    public void Configure(EntityTypeBuilder<TipologiaVoceFattura> builder)
    {
        builder.ToTable("TipologiaVoceFattura", "dbo");

        builder.HasKey(x => x.TipologiaVoceFatturaID);

        builder.Property(x => x.TipologiaVoceFatturaID)
            .HasColumnName("TipologiaVoceFatturaID");

        builder.Property(x => x.Descrizione)
            .HasColumnName("Descrizione")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.DescrizioneFatturazione)
            .HasColumnName("DescrizioneFatturazione")
            .HasMaxLength(250);

        builder.Property(x => x.CodArticoloFatturazione)
            .HasColumnName("CodArticoloFatturazione")
            .HasMaxLength(10);

        builder.Property(x => x.IsNotaCredito)
            .HasColumnName("IsNotaCredito")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CategoriaFatturaID)
            .HasColumnName("CategoriaFatturaID");

        builder.Property(x => x.CodiceCategoriaSegmento)
            .HasColumnName("CodiceCategoriaSegmento")
            .HasMaxLength(5);

        builder.Property(x => x.RipartizioneFatturatoID)
            .HasColumnName("RipartizioneFatturatoID")
            .HasMaxLength(1)
            .IsFixedLength();

        builder.Property(x => x.Attiva)
            .HasColumnName("Attiva")
            .IsRequired();

        builder.Property(x => x.CodArticoloFatturazioneSostitutivo)
            .HasColumnName("CodArticoloFatturazioneSostitutivo")
            .HasMaxLength(10);

        builder.Property(x => x.DataInserimento)
            .HasColumnName("DataInserimento");

        builder.Property(x => x.DataUltimaModifica)
            .HasColumnName("DataUltimaModifica");

        builder.Property(x => x.FatturazioneSuProprietaFisicaMezzo)
            .HasColumnName("FatturazioneSuProprietaFisicaMezzo");

        builder.Property(x => x.IvaID)
            .HasColumnName("IvaID");

        builder.HasOne(x => x.CategoriaFattura)
            .WithMany()
            .HasForeignKey(x => x.CategoriaFatturaID);

        builder.HasOne(x => x.CategoriaSegmento)
            .WithMany()
            .HasForeignKey(x => x.CodiceCategoriaSegmento);

        builder.HasOne(x => x.RipartizioneFatturato)
            .WithMany()
            .HasForeignKey(x => x.RipartizioneFatturatoID);

        builder.HasOne(x => x.Iva)
            .WithMany()
            .HasForeignKey(x => x.IvaID);

        builder.HasMany(x => x.AccessorioTipologie)
            .WithOne(x => x.TipologiaVoceFattura)
            .HasForeignKey(x => x.TipologiaVoceFatturaID);

        builder.HasMany(x => x.AccessorioTipologiePenale)
            .WithOne(x => x.TipologiaVoceFatturaPenale)
            .HasForeignKey(x => x.TipologiaVoceFatturaIDPenale);
    }
}