using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class CategoriaFatturaConfiguration : IEntityTypeConfiguration<CategoriaFattura>
{
    public void Configure(EntityTypeBuilder<CategoriaFattura> builder)
    {
        builder.ToTable("CategoriaFattura", "dbo");

        builder.HasKey(x => x.CategoriaFatturaID);

        builder.Property(x => x.CategoriaFatturaID)
            .HasColumnName("CategoriaFatturaID");

        builder.Property(x => x.Descrizione)
            .HasColumnName("Descrizione")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.IvaIDPredefinita)
            .HasColumnName("IvaIDPredefinita");

        builder.HasOne(x => x.IvaPredefinita)
            .WithMany()
            .HasForeignKey(x => x.IvaIDPredefinita);

        builder.HasMany(x => x.TipologieVoceFattura)
            .WithOne(x => x.CategoriaFattura)
            .HasForeignKey(x => x.CategoriaFatturaID);
    }
}