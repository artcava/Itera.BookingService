using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class WsUserGruppoConfiguration : IEntityTypeConfiguration<WsUserGruppo>
{
    public void Configure(EntityTypeBuilder<WsUserGruppo> builder)
    {
        builder.ToTable("WsUserGruppo", "dbo");
        builder.HasKey(x => x.WsUserGruppoID);
    }
}
