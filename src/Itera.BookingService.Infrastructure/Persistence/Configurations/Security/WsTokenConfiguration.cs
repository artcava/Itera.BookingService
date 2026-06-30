using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class WsTokenConfiguration : IEntityTypeConfiguration<WsToken>
{
    public void Configure(EntityTypeBuilder<WsToken> builder)
    {
        builder.ToTable("WsToken");
        builder.HasKey(x => x.WsTokenID);
        builder.Property(x => x.Token).IsRequired();
        builder.Property(x => x.BrandID).IsRequired();
    }
}
