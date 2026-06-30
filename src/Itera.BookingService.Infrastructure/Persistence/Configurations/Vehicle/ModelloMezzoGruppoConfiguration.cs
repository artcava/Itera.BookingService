using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class ModelloMezzoGruppoConfiguration : IEntityTypeConfiguration<ModelloMezzoGruppo>
{
    public void Configure(EntityTypeBuilder<ModelloMezzoGruppo> builder)
    {
        builder.ToTable("ModelloMezzoGruppo", "dbo");
        builder.HasKey(x => x.ModelloMezzoGruppoID);
    }
}
