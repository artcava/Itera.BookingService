using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class WsUserListinoConfiguration : IEntityTypeConfiguration<WsUserListino>
{
    public void Configure(EntityTypeBuilder<WsUserListino> builder)
    {
        builder.ToTable("WsUserListino", "dbo");
        builder.HasKey(x => x.WsUserListinoID);
    }
}
