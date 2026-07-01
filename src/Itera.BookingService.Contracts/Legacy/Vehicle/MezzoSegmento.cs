namespace Itera.BookingService.Contracts.Legacy.Vehicle;

public sealed class MezzoSegmento
{
    public int? ModelloMezzoID { get; set; }
    public string? Marca { get; set; }
    public string? ModelloDescr { get; set; }
    public string? NomeImmagine { get; set; }
    public short? Cilindrata { get; set; }
    public short? AlimentazioneModelloID { get; set; }
    public string? AlimentazioneDescr { get; set; }
    public string? Euro { get; set; }
    public byte? NumeroPosti { get; set; }
    public byte? NumeroPorte { get; set; }
    public bool? Autoradio { get; set; }
    public bool? AriaCondizionata { get; set; }
    public bool? Abs { get; set; }
    public bool? Airbag { get; set; }
    public short? CapacitaSerbatoio { get; set; }
    public short? Portata { get; set; }
    public short? VolumeCarico { get; set; }
    public double? AltezzaInterna { get; set; }
    public double? LarghezzaInterna { get; set; }
    public double? LunghezzaInterna { get; set; }
    public string? SegmentoDescrizione { get; set; }
    public string? CodiceSegmento { get; set; }
    public int? ModelloMezzoIDErs { get; set; }
    public short? IndexPricing { get; set; }
    public int? SegmentoModelloClasseID { get; set; }
    public string? SegmentoModelloClasseIDDescrizione { get; set; }
    public double? AltezzaEsterna { get; set; }
    public double? LarghezzaEsterna { get; set; }
    public double? LunghezzaEsterna { get; set; }
    public double? Passo { get; set; }
    public double? LarghezzaPassaruote { get; set; }
    public byte? NumeroPostiCarrozzina { get; set; }
    public byte? NumeroPostiMobility { get; set; }
    public bool PedanaSollevatriceDoppioBraccio { get; set; }
    public string? DescrizioneMobilitySitoWeb_ITA { get; set; }
    public string? DescrizioneMobilitySitoWeb_ENG { get; set; }
}
