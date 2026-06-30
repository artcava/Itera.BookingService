namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class ModelloMezzo
{
    public int ModelloMezzoID { get; set; }
    public string? CodiceSegmento { get; set; }
    public string? CodiceCategoria { get; set; }
    public short MarcaID { get; set; }
    public short AlimentazioneModelloID { get; set; }
    public short? Serbatoio { get; set; }
    public string Descrizione { get; set; } = string.Empty;
    public string? NomeImmagine { get; set; }
    public double? Passo { get; set; }
    public double? LunghezzaEsterna { get; set; }
    public double? AltezzaEsterna { get; set; }
    public double? LarghezzaEsterna { get; set; }
    public short? Peso { get; set; }
    public double? LarghezzaPassaruote { get; set; }
    public double? LunghezzaInterna { get; set; }
    public double? AltezzaInterna { get; set; }
    public double? LarghezzaInterna { get; set; }
    public short? Portata { get; set; }
    public short? VolumeCarico { get; set; }
    public short? NumeroPallets { get; set; }
    public double? AltezzaCarico { get; set; }
    public double? MisurePortaPosteriore { get; set; }
    public double? MisurePortaLaterale { get; set; }
    public short? Cilindrata { get; set; }
    public string? Euro { get; set; }
    public int? Km { get; set; }
    public short? CV { get; set; }
    public short? CapacitaSerbatoio { get; set; }
    public byte? NumeroPorte { get; set; }
    public byte? NumeroPosti { get; set; }
    public bool? Autoradio { get; set; }
    public bool? AriaCondizionata { get; set; }
    public bool? Airbag { get; set; }
    public bool? Abs { get; set; }
    public bool? Eps { get; set; }
    public string? MisuraGomme { get; set; }
    public bool? RuotaScorta { get; set; }
    public bool? KitGonfiaggio { get; set; }
    public DateTime DataModifica { get; set; }
    public int? KmTagliandoFreni { get; set; }
    public int? SogliaKmTagliandoFreni { get; set; }
    public int? KmTagliandoOlio { get; set; }
    public int? SogliaKmTagliandoOlio { get; set; }
    public bool? Ruotino { get; set; }
    public string? NomeFileSchedaTecnica { get; set; }
    public bool? VisibilitaSito { get; set; }
    public string? ACRISSCode { get; set; }
    public byte? NumeroPostiCarrozzina { get; set; }
    public byte? NumeroPostiMobility { get; set; }
    public bool PedanaSollevatriceDoppioBraccio { get; set; }
    public string? DescrizioneMobilitySitoWeb_ITA { get; set; }
    public string? DescrizioneMobilitySitoWeb_ENG { get; set; }
}
