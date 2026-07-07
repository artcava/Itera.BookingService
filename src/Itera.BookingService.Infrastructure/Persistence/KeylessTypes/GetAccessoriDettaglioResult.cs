using Microsoft.EntityFrameworkCore;

namespace Itera.BookingService.Infrastructure.Persistence.KeylessTypes;

[Keyless]
public sealed class GetAccessoriDettaglioResult
{
    public short AccessorioTipologiaID { get; set; }
    public byte TipologiaVoceFatturaID { get; set; }
    public string? Codice { get; set; }
    public string? DescrizioneTipologiaVoceFattura { get; set; }
    public short? AccessorioCategoriaID { get; set; }
    public string? CodiceCategoria { get; set; }
    public string? DescrizioneCategoria { get; set; }
    public bool Obbligatorio { get; set; }
    public bool Preselezionato { get; set; }
    public bool PrepagamentoWeb { get; set; }
    public string? MomentoVendibilita { get; set; }
    public string? CodiceSegmento { get; set; }
    public short? IvaID { get; set; }
}