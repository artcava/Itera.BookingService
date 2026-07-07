using Itera.BookingService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class AccessorioTipologiaConfiguration : IEntityTypeConfiguration<AccessorioTipologia>
{
    public void Configure(EntityTypeBuilder<AccessorioTipologia> builder)
    {
        builder.ToTable("AccessorioTipologia", "dbo");

        builder.HasKey(x => x.AccessorioTipologiaID);

        builder.Property(x => x.AccessorioTipologiaID)
            .HasColumnName("AccessorioTipologiaID")
            .UseIdentityColumn();

        builder.Property(x => x.AccessorioCategoriaID)
            .HasColumnName("AccessorioCategoriaID")
            .IsRequired();

        builder.Property(x => x.Codice)
            .HasColumnName("Codice")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.BrandID)
            .HasColumnName("BrandID");

        builder.Property(x => x.Obbligatorio)
            .HasColumnName("Obbligatorio")
            .IsRequired();

        builder.Property(x => x.IvaID)
            .HasColumnName("IvaID");

        builder.Property(x => x.OneriAptRw)
            .HasColumnName("OneriAptRw")
            .IsRequired();

        builder.Property(x => x.ImportoForzato)
            .HasColumnName("ImportoForzato")
            .IsRequired();

        builder.Property(x => x.InseribileFiliale)
            .HasColumnName("InseribileFiliale")
            .IsRequired();

        builder.Property(x => x.ModificabileFiliale)
            .HasColumnName("ModificabileFiliale")
            .IsRequired();

        builder.Property(x => x.PrepagamentoWeb)
            .HasColumnName("PrepagamentoWeb")
            .IsRequired();

        builder.Property(x => x.PrepagamentoContactCenter)
            .HasColumnName("PrepagamentoContactCenter")
            .IsRequired();

        builder.Property(x => x.VendibilitaWeb)
            .HasColumnName("VendibilitaWeb")
            .IsRequired();

        builder.Property(x => x.VendibilitaContactCenter)
            .HasColumnName("VendibilitaContactCenter")
            .IsRequired();

        builder.Property(x => x.VendibilitaFiliale)
            .HasColumnName("VendibilitaFiliale")
            .IsRequired();

        builder.Property(x => x.TipologiaVoceFatturaID)
            .HasColumnName("TipologiaVoceFatturaID")
            .IsRequired();

        builder.Property(x => x.PercentualeShort)
            .HasColumnName("PercentualeShort")
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.PercentualeOpen)
            .HasColumnName("PercentualeOpen")
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.PercentualeAccessorio)
            .HasColumnName("PercentualeAccessorio")
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.StatoID)
            .HasColumnName("StatoID")
            .IsRequired();

        builder.Property(x => x.DataInizioValidita)
            .HasColumnName("DataInizioValidita");

        builder.Property(x => x.DataFineValidita)
            .HasColumnName("DataFineValidita");

        builder.Property(x => x.SplitInFattura)
            .HasColumnName("SplitInFattura")
            .IsRequired();

        builder.Property(x => x.PercentualeSplit)
            .HasColumnName("PercentualeSplit")
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.ImportoPenale)
            .HasColumnName("ImportoPenale")
            .HasColumnType("money");

        builder.Property(x => x.TipologiaVoceFatturaIDPenale)
            .HasColumnName("TipologiaVoceFatturaIDPenale");

        builder.Property(x => x.Preselezionato)
            .HasColumnName("Preselezionato")
            .IsRequired();

        builder.Property(x => x.MomentoVendibilita)
            .HasColumnName("MomentoVendibilita")
            .HasMaxLength(2)
            .IsFixedLength();

        builder.Property(x => x.AccessorioTipologiaIDPenale)
            .HasColumnName("AccessorioTipologiaIDPenale");

        builder.Property(x => x.Vendibile)
            .HasColumnName("Vendibile")
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(x => x.AccessorioCategoria)
            .WithMany()
            .HasForeignKey(x => x.AccessorioCategoriaID);

        builder.HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandID);

        builder.HasOne(x => x.Iva)
            .WithMany()
            .HasForeignKey(x => x.IvaID);

        builder.HasOne(x => x.TipologiaVoceFattura)
            .WithMany()
            .HasForeignKey(x => x.TipologiaVoceFatturaID);

        builder.HasOne(x => x.TipologiaVoceFatturaPenale)
            .WithMany()
            .HasForeignKey(x => x.TipologiaVoceFatturaIDPenale);

        builder.HasOne(x => x.AccessorioTipologiaPenale)
            .WithMany(x => x.AccessoriTipologiaPenaleFigli)
            .HasForeignKey(x => x.AccessorioTipologiaIDPenale);
    }
}