using System.ComponentModel.DataAnnotations;

namespace Locatic.Models;

public class Voiture
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Immatriculation")]
    public string Immatriculation { get; set; } = "";

    [Range(1900, 2100)]
    [Display(Name = "Année")]
    public int Annee { get; set; }

    [Range(0.01, 9999)]
    [Display(Name = "Tarif journalier")]
    public decimal TarifJournalier { get; set; }

    [Range(1, 20)]
    [Display(Name = "Nombre de places")]
    public int NombrePlaces { get; set; }

    [Required]
    [Display(Name = "Carburant")]
    public string Carburant { get; set; } = "";

    [Required]
    [Display(Name = "Modèle")]
    public int ModeleId { get; set; }

    public Modele? Modele { get; set; }

    public List<Reservation>? Reservations { get; set; }
}