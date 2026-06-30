using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class ModelloMezzoConfiguration : IEntityTypeConfiguration<ModelloMezzo>
{
    public void Configure(EntityTypeBuilder<ModelloMezzo> builder)
    {
        builder.ToTable("ModelloMezzo", "dbo");
        builder.HasKey(x => x.ModelloMezzoID);
        builder.Property(x => x.CodiceSegmento).HasMaxLength(3);
        builder.Property(x => x.CodiceCategoria).HasMaxLength(5);
        builder.Property(x => x.Descrizione).HasMaxLength(50).IsRequired();
        builder.Property(x => x.NomeImmagine).HasMaxLength(100);
        builder.Property(x => x.Euro).HasMaxLength(2).IsFixedLength();
        builder.Property(x => x.MisuraGomme).HasMaxLength(50);
        builder.Property(x => x.NomeFileSchedaTecnica).HasMaxLength(150);
        builder.Property(x => x.ACRISSCode).HasMaxLength(100);
        builder.Property(x => x.DescrizioneMobilitySitoWeb_ITA).HasColumnType("nvarchar(max)");
        builder.Property(x => x.DescrizioneMobilitySitoWeb_ENG).HasColumnType("nvarchar(max)");
    }
}
