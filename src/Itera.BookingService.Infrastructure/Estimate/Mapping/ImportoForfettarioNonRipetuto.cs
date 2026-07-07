namespace Itera.BookingService.Infrastructure.Estimate.Mapping;

public sealed class ImportoForfettarioNonRipetuto : TariffaRdvBase
{
    public override TipoImporto TipoImporto => TipoImporto.ForfettarioNonRipetuto;

    protected override bool IsValida => GiorniNoleggio >= MinGiorniApplicabilita;

    protected override decimal GetImporto(int giorniNoleggio)
    {
        var giorniExtra = giorniNoleggio > MaxGiorniApplicabilita ? giorniNoleggio : 0;
        return ImportoFisso + ImportoGiornoExtra.GetValueOrDefault(0) * giorniExtra;
    }

    protected override decimal GetImportoUnitario()
    {
        return (ImportoCalcolato / (GiorniNoleggio < 30 ? GiorniNoleggio : 30)).Floor();
    }
}