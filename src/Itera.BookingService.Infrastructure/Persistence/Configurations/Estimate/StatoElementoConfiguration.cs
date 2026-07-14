using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public sealed class StatoElementoConfiguration : IEntityTypeConfiguration<StatoElemento>
{
    public void Configure(EntityTypeBuilder<StatoElemento> builder)
    {
        builder.ToTable("StatoElemento", "dbo");

        builder.HasKey(x => new { x.Codice, x.Tipologia });

        builder.Property(x => x.Codice)
            .HasColumnName("Codice")
            .IsRequired();

        builder.Property(x => x.Descrizione)
            .HasColumnName("Descrizione")
            .HasMaxLength(20);

        builder.Property(x => x.Tipologia)
            .HasColumnName("Tipologia")
            .HasMaxLength(10)
            .IsRequired();
    }
}