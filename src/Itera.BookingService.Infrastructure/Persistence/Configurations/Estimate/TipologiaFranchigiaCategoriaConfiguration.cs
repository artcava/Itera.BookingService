using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public sealed class TipologiaFranchigiaCategoriaConfiguration : IEntityTypeConfiguration<TipologiaFranchigiaCategoria>
{
    public void Configure(EntityTypeBuilder<TipologiaFranchigiaCategoria> builder)
    {
        builder.ToTable("TipologiaFranchigiaCategoria", "dbo");

        builder.HasKey(x => x.TipologiaFranchigiaCategoriaID);

        builder.Property(x => x.TipologiaFranchigiaCategoriaID)
            .HasColumnName("TipologiaFranchigiaCategoriaID")
            .IsRequired();

        builder.Property(x => x.Descrizione)
            .HasColumnName("Descrizione")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Codice)
            .HasColumnName("Codice")
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(x => x.StatoID)
            .HasColumnName("StatoID")
            .IsRequired();
    }
}