using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations.Estimate;

public class TariffaRdvConfiguration : IEntityTypeConfiguration<TariffaRdv>
{
    public void Configure(EntityTypeBuilder<TariffaRdv> builder)
    {
        builder.ToTable("TariffaRdv", "pricing");

        builder.HasKey(x => x.TariffaRdvID);

        builder.Property(x => x.TariffaRdvID)
            .HasColumnName("TariffaRdvID")
            .UseIdentityColumn();

        builder.Property(x => x.TariffarioID)
            .HasColumnName("TariffarioID")
            .IsRequired();

        builder.Property(x => x.AccessorioTipologiaID)
            .HasColumnName("AccessorioTipologiaID")
            .IsRequired();

        builder.Property(x => x.DataStart)
            .HasColumnName("DataStart")
            .IsRequired();

        builder.Property(x => x.DataEnd)
            .HasColumnName("DataEnd")
            .IsRequired();

        builder.Property(x => x.BreakEven)
            .HasColumnName("BreakEven")
            .IsRequired();

        builder.Property(x => x.MinGiorniApplicabilita)
            .HasColumnName("MinGiorniApplicabilita")
            .IsRequired();

        builder.Property(x => x.MaxGiorniApplicabilita)
            .HasColumnName("MaxGiorniApplicabilita")
            .IsRequired();

        builder.Property(x => x.Percentuale)
            .HasColumnName("Percentuale")
            .HasPrecision(5, 2);

        builder.Property(x => x.ImportoFisso)
            .HasColumnName("ImportoFisso")
            .HasColumnType("money");

        builder.Property(x => x.TipoImporto)
            .HasColumnName("TipoImporto")
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.ImportoGiornoExtra)
            .HasColumnName("ImportoGiornoExtra")
            .HasColumnType("money");

        builder.Property(x => x.ImportoMinAddebitabile)
            .HasColumnName("ImportoMinAddebitabile")
            .HasColumnType("money");

        builder.Property(x => x.ImportoMaxAddebitabile)
            .HasColumnName("ImportoMaxAddebitabile")
            .HasColumnType("money");

        builder.Property(x => x.MaxGiorniAddebitabili)
            .HasColumnName("MaxGiorniAddebitabili");

        builder.Property(x => x.Tolleranza)
            .HasColumnName("Tolleranza")
            .HasPrecision(5, 2);

        builder.Property(x => x.StatoInclusione)
            .HasColumnName("StatoInclusione")
            .HasMaxLength(3);

        builder.Property(x => x.Incasso)
            .HasColumnName("Incasso")
            .HasMaxLength(3);

        builder.Property(x => x.StatoID)
            .HasColumnName("StatoID")
            .IsRequired();

        builder.Property(x => x.DataInserimento)
            .HasColumnName("DataInserimento")
            .IsRequired();

        builder.Property(x => x.DataUltimaModifica)
            .HasColumnName("DataUltimaModifica");

        builder.HasOne(x => x.Tariffario)
            .WithMany(x => x.TariffeRdv)
            .HasForeignKey(x => x.TariffarioID);

        builder.HasOne(x => x.AccessorioTipologia)
            .WithMany(x => x.TariffeRdv)
            .HasForeignKey(x => x.AccessorioTipologiaID);
    }
}