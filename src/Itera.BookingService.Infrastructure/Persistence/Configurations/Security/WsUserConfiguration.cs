using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class WsUserConfiguration : IEntityTypeConfiguration<WsUser>
{
    public void Configure(EntityTypeBuilder<WsUser> builder)
    {
        builder.ToTable("WsUser");
        builder.HasKey(x => x.WsUserID);
        builder.Property(x => x.WsUserID).ValueGeneratedNever();
        builder.Property(x => x.Username).HasMaxLength(50).IsRequired();
        builder.Property(x => x.SecretWord).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Descrizione).HasMaxLength(250);

        builder.HasMany(x => x.WsTokens)
               .WithOne(t => t.WsUser)
               .HasForeignKey(t => t.WsUserID);
    }
}
