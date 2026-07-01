using Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations.Estimate;

public sealed class ListinoConfiguration : IEntityTypeConfiguration<Listino>
{
    public void Configure(EntityTypeBuilder<Listino> builder)
    {
        builder.ToTable("Listino", "dbo");
        builder.HasKey(x => x.ListinoId);
        builder.Property(x => x.ListinoId).HasColumnName("ListinoID");
        builder.Property(x => x.InizioValidita).HasColumnName("InizioValidita");
        builder.Property(x => x.FineValidita).HasColumnName("FineValidita");
        builder.Property(x => x.SempreAttivo).HasColumnName("SempreAttivo").IsRequired();

        builder.HasMany(x => x.ListinoGiorni)
               .WithOne(x => x.Listino)
               .HasForeignKey(x => x.ListinoId);

        builder.HasMany(x => x.ListinoFiliali)
               .WithOne()
               .HasForeignKey(x => x.ListinoId);
    }
}
