using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itera.BookingService.Infrastructure.Persistence.Configurations;

public class GetFilialeInfoWs2ResultConfiguration : IEntityTypeConfiguration<GetFilialeInfoWs2Result>
{
    public void Configure(EntityTypeBuilder<GetFilialeInfoWs2Result> builder)
    {
        builder.HasNoKey();
        builder.ToView(null);
    }
}

public class GetFilialiFatturazioneClienteWsResultConfiguration : IEntityTypeConfiguration<GetFilialiFatturazioneClienteWsResult>
{
    public void Configure(EntityTypeBuilder<GetFilialiFatturazioneClienteWsResult> builder)
    {
        builder.HasNoKey();
        builder.ToView(null);
    }
}

public class GetMezziWsResultConfiguration : IEntityTypeConfiguration<GetMezziWsResult>
{
    public void Configure(EntityTypeBuilder<GetMezziWsResult> builder)
    {
        builder.HasNoKey();
        builder.ToView(null);
    }
}

public class GetListinoValoriResultConfiguration : IEntityTypeConfiguration<GetListinoValoriResult>
{
    public void Configure(EntityTypeBuilder<GetListinoValoriResult> builder)
    {
        builder.HasNoKey();
        builder.ToView(null);
    }
}

public class GetKmInfoMultiResultConfiguration : IEntityTypeConfiguration<GetKmInfoMultiResult>
{
    public void Configure(EntityTypeBuilder<GetKmInfoMultiResult> builder)
    {
        builder.HasNoKey();
        builder.ToView(null);
    }
}

public class VwListinoFranchigiaResultConfiguration : IEntityTypeConfiguration<VwListinoFranchigiaResult>
{
    public void Configure(EntityTypeBuilder<VwListinoFranchigiaResult> builder)
    {
        builder.HasNoKey();
        builder.ToView("VwListinoFranchigia", "dbo");
    }
}