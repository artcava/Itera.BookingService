namespace Itera.BookingService.Infrastructure.Estimate.Mapping;

public sealed class ImportoForfettario : TariffaRdvBase
{
    public override TipoImporto TipoImporto => TipoImporto.Forfettario;

    protected override bool IsValida => GiorniNoleggio >= MinGiorniApplicabilita;

    protected override decimal GetImporto(int giorniCompetenza)
    {
        if (PeriodoCompetenza.IsRateoGiorniExtra() &&
            MomentoVendibilitaPreventivo == MomentiVendibilita.Chiusura &&
            MomentoVendibilitaAccessorio == MomentiVendibilita.Creazione)
        {
            if (ImportoGiornoExtra.HasValue)
                return ImportoGiornoExtra.Value * giorniCompetenza;
            if (MaxGiorniApplicabilita > GiorniNoleggio)
                return ImportoFisso;

            return (ImportoFisso / MaxGiorniApplicabilita * giorniCompetenza).Floor();
        }

        if (!(MomentoVendibilitaPreventivo == MomentiVendibilita.Chiusura &&
              MomentoVendibilitaAccessorio == MomentiVendibilita.Chiusura))
        {
            giorniCompetenza = giorniCompetenza < 30 ? giorniCompetenza : 30;
        }

        var numeroForfait = giorniCompetenza / MaxGiorniApplicabilita > 0
            ? giorniCompetenza / MaxGiorniApplicabilita
            : 1;

        var giorniExtra = giorniCompetenza > MaxGiorniApplicabilita
            ? giorniCompetenza % MaxGiorniApplicabilita
            : 0;

        return ImportoFisso * numeroForfait + ImportoGiornoExtra.GetValueOrDefault(0) * giorniExtra;
    }

    protected override decimal GetImportoUnitario()
    {
        if (PeriodoCompetenza.IsRateoGiorniExtra() &&
            MomentoVendibilitaPreventivo == MomentiVendibilita.Chiusura &&
            MomentoVendibilitaAccessorio == MomentiVendibilita.Creazione)
        {
            if (ImportoGiornoExtra.HasValue)
                return ImportoGiornoExtra.Value;
            if (MaxGiorniApplicabilita > GiorniNoleggio)
                return ImportoFisso;

            return (ImportoFisso / MaxGiorniApplicabilita).Floor();
        }

        return (ImportoCalcolato / (GiorniNoleggio < 30 ? GiorniNoleggio : 30)).Floor();
    }
}