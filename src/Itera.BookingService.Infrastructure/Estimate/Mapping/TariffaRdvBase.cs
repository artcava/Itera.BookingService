namespace Itera.BookingService.Infrastructure.Estimate.Mapping;

public abstract class TariffaRdvBase
{
    public int TariffarioID { get; set; }
    public string? DescrizioneTariffario { get; set; }
    public short AccessorioTipologiaID { get; set; }
    public string? DescrizioneAccessorio { get; set; }
    public bool? IsNotaCredito { get; set; }
    public byte? TipologiaVoceFatturaID { get; set; }
    public DateTime DataStart { get; set; }
    public DateTime DataEnd { get; set; }
    public short BreakEven { get; set; }
    public short MinGiorniApplicabilita { get; set; }
    public short MaxGiorniApplicabilita { get; set; }
    public decimal? Percentuale { get; set; }
    public decimal ImportoFisso { get; set; }
    public decimal? ImportoGiornoExtra { get; set; }
    public decimal? ImportoMinAddebitabile { get; set; }
    public decimal? ImportoMaxAddebitabile { get; set; }
    public short? MaxGiorniAddebitabili { get; set; }
    public decimal? Tolleranza { get; set; }
    public string? StatoInclusione { get; set; }
    public string? Incasso { get; set; }

    public int GiorniNoleggio { get; set; }
    public string? MomentoVendibilitaPreventivo { get; set; }
    public string? MomentoVendibilitaAccessorio { get; set; }
    public PeriodoCompetenza PeriodoCompetenza { get; set; } = PeriodoCompetenza.None();

    public abstract TipoImporto TipoImporto { get; }

    public decimal ImportoCalcolato => CalcolaImporto();
    public decimal ImportoUnitario => GetImportoUnitario();

    protected abstract bool IsValida { get; }
    protected abstract decimal GetImporto(int giorniNoleggio);
    protected abstract decimal GetImportoUnitario();

    protected decimal CalcolaImporto()
    {
        if (!IsValida)
            throw new InvalidOperationException("Regola non calcolabile");

        var giorniCompetenza = GiorniNoleggio;

        if (PeriodoCompetenza.IsRateoGiorniExtra() &&
            MomentoVendibilitaPreventivo == MomentiVendibilita.Chiusura &&
            MomentoVendibilitaAccessorio == MomentiVendibilita.Creazione)
        {
            giorniCompetenza = PeriodoCompetenza.GiorniPeriodo;
        }

        var importo = GetImporto(giorniCompetenza);

        if (ImportoMinAddebitabile.HasValue && importo < ImportoMinAddebitabile.Value)
            return ImportoMinAddebitabile.Value;

        if (ImportoMaxAddebitabile.HasValue && importo > ImportoMaxAddebitabile.Value)
        {
            if (MaxGiorniAddebitabili.HasValue)
                return new[] { ImportoMaxAddebitabile.Value, GetImporto(MaxGiorniAddebitabili.Value) }.Min();

            return ImportoMaxAddebitabile.Value;
        }

        if (MaxGiorniAddebitabili.HasValue && GiorniNoleggio > MaxGiorniAddebitabili.Value)
            return new[] { importo, GetImporto(MaxGiorniAddebitabili.Value) }.Min();

        return importo;
    }
}