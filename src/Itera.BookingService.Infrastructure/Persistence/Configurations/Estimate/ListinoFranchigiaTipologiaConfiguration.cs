using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public sealed class ListinoFranchigiaTipologiaConfiguration : IEntityTypeConfiguration<ListinoFranchigiaTipologia>
{
    public void Configure(EntityTypeBuilder<ListinoFranchigiaTipologia> builder)
    {
        builder.ToTable("ListinoFranchigiaTipologia", "dbo");

        builder.HasKey(x => new { x.ListinoID, x.TipologiaFranchigiaID });

        builder.Property(x => x.ListinoID)
            .HasColumnName("ListinoID")
            .IsRequired();

        builder.Property(x => x.TipologiaFranchigiaID)
            .HasColumnName("TipologiaFranchigiaID")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.StatoID)
            .HasColumnName("StatoID")
            .IsRequired();

        builder.Property(x => x.StatoInclusione)
            .HasColumnName("StatoInclusione")
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength();
    }
}