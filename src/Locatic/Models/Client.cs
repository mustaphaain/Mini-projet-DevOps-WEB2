using System.ComponentModel.DataAnnotations;

namespace Locatic.Models;

public class Client
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Nom")]
    public string Nom { get; set; } = "";

    [Required]
    [Display(Name = "Prénom")]
    public string Prenom { get; set; } = "";

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Display(Name = "Téléphone")]
    public string Telephone { get; set; } = "";

    public List<Reservation>? Reservations { get; set; }
}