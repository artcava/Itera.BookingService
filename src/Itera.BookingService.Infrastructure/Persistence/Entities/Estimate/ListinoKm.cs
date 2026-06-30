using Microsoft.EntityFrameworkCore;

namespace Itera.BookingService.Infrastructure.Persistence.Entities.Estimate;

/// <summary>
/// Rappresenta la tabella ListinoKm del DB legacy.
/// Contiene le opzioni km associate a un listino/categoria.
/// </summary>
[Index(nameof(ListinoId), nameof(CategoriaId))]
public sealed class ListinoKm
{
    /// <summary>PK — KmID (es. "100", "200", "UNLI").</summary>
    public string KmId { get; set; } = default!;

    /// <summary>Descrizione dell'opzione km.</summary>
    public string? Descrizione { get; set; }

    /// <summary>FK verso il listino di riferimento.</summary>
    public int ListinoId { get; set; }

    /// <summary>Codice categoria veicolo (es. "ECO").</summary>
    public string? CategoriaId { get; set; }

    /// <summary>Fascia oraria di ritiro associata a questa opzione.</summary>
    public int? FasciaOrarioRitiro { get; set; }

    /// <summary>Fascia oraria di consegna associata a questa opzione.</summary>
    public int? FasciaOrarioConsegna { get; set; }

    /// <summary>Indica se questa opzione è quella di default per il listino.</summary>
    public bool Selected { get; set; }

    /// <summary>FK verso la filiale.</summary>
    public int FilialeId { get; set; }
}
