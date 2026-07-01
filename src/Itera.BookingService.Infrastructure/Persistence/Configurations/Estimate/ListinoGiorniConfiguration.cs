using Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations.Estimate;

public sealed class ListinoGiorniConfiguration : IEntityTypeConfiguration<ListinoGiorni>
{
    public void Configure(EntityTypeBuilder<ListinoGiorni> builder)
    {
        builder.ToTable("ListinoGiorni", "dbo");
        builder.HasKey(x => x.ListinoGiorniId);
        builder.Property(x => x.ListinoGiorniId).HasColumnName("ListinoGiorniID");
        builder.Property(x => x.ListinoId).HasColumnName("ListinoID");
        builder.Property(x => x.Codice).HasMaxLength(30).IsRequired();
        builder.Property(x => x.CodiceCategoria).HasMaxLength(5).IsRequired();
        builder.Property(x => x.Descrizione).HasMaxLength(50).IsRequired();
        builder.Property(x => x.FasciaStart).HasColumnName("FasciaStart").IsRequired();
        builder.Property(x => x.FasciaEnd).HasColumnName("FasciaEnd").IsRequired();
        builder.Property(x => x.Ordinamento).HasColumnName("Ordinamento").IsRequired();
        builder.Property(x => x.IsVisible).HasColumnName("IsVisible");

        builder.HasMany(x => x.ListinoKm)
               .WithOne(x => x.ListinoGiorni)
               .HasForeignKey(x => x.ListinoGiorniId);
    }
}
