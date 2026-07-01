using Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations.Estimate;

public sealed class ListinoFilialeConfiguration : IEntityTypeConfiguration<ListinoFiliale>
{
    public void Configure(EntityTypeBuilder<ListinoFiliale> builder)
    {
        builder.ToTable("ListinoFiliale", "dbo");
        builder.HasKey(x => new { x.FilialeId, x.ListinoId });
        builder.Property(x => x.FilialeId).HasColumnName("FilialeID");
        builder.Property(x => x.ListinoId).HasColumnName("ListinoID");
    }
}
