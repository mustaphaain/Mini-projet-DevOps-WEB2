using System.ComponentModel.DataAnnotations;

namespace Locatic.Models;

public class Reservation
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Client")]
    public int ClientId { get; set; }
    public Client? Client { get; set; }

    [Required]
    [Display(Name = "Voiture")]
    public int VoitureId { get; set; }
    public Voiture? Voiture { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date début")]
    public DateTime DateDebut { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date fin")]
    public DateTime DateFin { get; set; }

    [Display(Name = "Prix total")]
    public decimal PrixTotal { get; set; }
}