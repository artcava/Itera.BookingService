using Itera.BookingService.Infrastructure.Persistence.Entities;
using Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class ListinoConfiguration : IEntityTypeConfiguration<Listino>
{
    public void Configure(EntityTypeBuilder<Listino> builder)
    {
        builder.ToTable("Listino", "dbo");
        builder.HasKey(x => x.ListinoID);
    }
}

public class ListinoBrandConfiguration : IEntityTypeConfiguration<ListinoBrand>
{
    public void Configure(EntityTypeBuilder<ListinoBrand> builder)
    {
        builder.ToTable("ListinoBrand", "dbo");
        builder.HasKey(x => x.ListinoBrandID);
    }
}

public class ListinoGiorniConfiguration : IEntityTypeConfiguration<ListinoGiorni>
{
    public void Configure(EntityTypeBuilder<ListinoGiorni> builder)
    {
        builder.ToTable("ListinoGiorni", "dbo");
        builder.HasKey(x => x.ListinoGiorniID);
    }
}

public class ListinoKmConfiguration : IEntityTypeConfiguration<ListinoKm>
{
    public void Configure(EntityTypeBuilder<ListinoKm> builder)
    {
        builder.ToTable("ListinoKm", "dbo");
        builder.HasKey(x => x.ListinoKmID);
    }
}

public class KmConfiguration : IEntityTypeConfiguration<Km>
{
    public void Configure(EntityTypeBuilder<Km> builder)
    {
        builder.ToTable("Km", "dbo");
        builder.HasKey(x => x.KmID);
    }
}

public class ListinoValoriConfiguration : IEntityTypeConfiguration<ListinoValori>
{
    public void Configure(EntityTypeBuilder<ListinoValori> builder)
    {
        builder.ToTable("ListinoValori", "dbo");
        builder.HasKey(x => x.ListinoValoriID);
    }
}

public class ListinoFranchigiaConfiguration : IEntityTypeConfiguration<ListinoFranchigia>
{
    public void Configure(EntityTypeBuilder<ListinoFranchigia> builder)
    {
        builder.ToTable("ListinoFranchigia", "dbo");
        builder.HasKey(x => x.ListinoFranchigiaID);
    }
}

public class TipologiaFranchigiaConfiguration : IEntityTypeConfiguration<TipologiaFranchigia>
{
    public void Configure(EntityTypeBuilder<TipologiaFranchigia> builder)
    {
        builder.ToTable("TipologiaFranchigia", "dbo");
        builder.HasKey(x => x.TipologiaFranchigiaID);
    }
}

public class AccordoCommercialeListinoConfiguration : IEntityTypeConfiguration<AccordoCommercialeListino>
{
    public void Configure(EntityTypeBuilder<AccordoCommercialeListino> builder)
    {
        builder.ToTable("AccordoCommercialeListino", "dbo");
        builder.HasKey(x => x.AccordoCommercialeListinoID);
    }
}

public class RegolaDiVenditaListinoConfiguration : IEntityTypeConfiguration<RegolaDiVenditaListino>
{
    public void Configure(EntityTypeBuilder<RegolaDiVenditaListino> builder)
    {
        builder.ToTable("RegolaDiVenditaListino", "dbo");
        builder.HasKey(x => x.RegolaDiVenditaListinoID);
    }
}

public class PreventivoConfiguration : IEntityTypeConfiguration<Preventivo>
{
    public void Configure(EntityTypeBuilder<Preventivo> builder)
    {
        builder.ToTable("Preventivo", "dbo");
        builder.HasKey(x => x.PreventivoID);
    }
}

public class WsTokenPreventivoConfiguration : IEntityTypeConfiguration<WsTokenPreventivo>
{
    public void Configure(EntityTypeBuilder<WsTokenPreventivo> builder)
    {
        builder.ToTable("WsTokenPreventivo", "dbo");
        builder.HasKey(x => x.WsTokenPreventivoID);
    }
}
