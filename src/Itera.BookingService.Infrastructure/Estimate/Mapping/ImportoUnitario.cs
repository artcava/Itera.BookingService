namespace Itera.BookingService.Infrastructure.Estimate.Mapping;

public sealed class ImportoUnitario : TariffaRdvBase
{
    public override TipoImporto TipoImporto => TipoImporto.Unitario;

    protected override bool IsValida =>
        GiorniNoleggio >= MinGiorniApplicabilita &&
        GiorniNoleggio <= MaxGiorniApplicabilita;

    protected override decimal GetImporto(int giorniCompetenza)
    {
        if (!(MomentoVendibilitaPreventivo == MomentiVendibilita.Chiusura &&
              MomentoVendibilitaAccessorio == MomentiVendibilita.Chiusura))
        {
            giorniCompetenza = giorniCompetenza < 30 ? giorniCompetenza : 30;
        }

        return ImportoFisso * giorniCompetenza;
    }

    protected override decimal GetImportoUnitario()
    {
        if (PeriodoCompetenza.IsRateoGiorniExtra())
            return (ImportoCalcolato / PeriodoCompetenza.GiorniPeriodo).Floor();

        return (ImportoCalcolato / (GiorniNoleggio < 30 ? GiorniNoleggio : 30)).Floor();
    }
}