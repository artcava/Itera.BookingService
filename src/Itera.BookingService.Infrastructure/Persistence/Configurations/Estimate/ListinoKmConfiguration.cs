using Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations.Estimate;

public sealed class ListinoKmConfiguration : IEntityTypeConfiguration<ListinoKm>
{
    public void Configure(EntityTypeBuilder<ListinoKm> builder)
    {
        builder.ToTable("ListinoKm", "dbo");
        builder.HasKey(x => x.ListinoKmId);
        builder.Property(x => x.ListinoKmId).HasColumnName("ListinoKmID").UseIdentityColumn();
        builder.Property(x => x.ListinoGiorniId).HasColumnName("ListinoGiorniID").IsRequired();
        builder.Property(x => x.Km).HasColumnName("Km").IsRequired();
        builder.Property(x => x.Ordinamento).HasColumnName("Ordinamento").IsRequired();
        builder.Property(x => x.IsVisible).HasColumnName("IsVisible");
    }
}
