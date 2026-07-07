namespace Itera.BookingService.Infrastructure.Persistence.Entities;

public class Iva
{
    public short IvaId { get; set; }
    public decimal Percentuale { get; set; }
    public DateTime ValidaDal { get; set; }
    public DateTime? ValidaAl { get; set; }
    public string? Descrizione { get; set; }
    public int? Ordinamento { get; set; }
    public bool Sistema { get; set; }
    public bool? FatturaOmaggio { get; set; }
    public string? DescrizioneSuFattura { get; set; }
    public string? NotaAggiuntivaSuFatturaIta { get; set; }
    public string? NotaAggiuntivaSuFatturaEng { get; set; }
    public short? IvaIdSostituzione365Plus { get; set; }
    public bool SplitPayment { get; set; }

    public Iva? IvaSostituzione365Plus { get; set; }
    public ICollection<Iva> IvaSostituzioni365Plus { get; set; } = [];
}