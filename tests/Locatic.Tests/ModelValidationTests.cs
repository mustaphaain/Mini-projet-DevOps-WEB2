using System.ComponentModel.DataAnnotations;
using Locatic.Models;
using Xunit;

namespace Locatic.Tests;

public class ModelValidationTests
{
    [Fact]
    public void Voiture_WithInvalidAnnee_FailsValidation()
    {
        var voiture = new Voiture
        {
            Immatriculation = "AB-123-CD",
            Annee = 1800,
            TarifJournalier = 50,
            NombrePlaces = 5,
            Carburant = "Essence",
            ModeleId = 1
        };

        var results = Validate(voiture);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void Marque_WithEmptyNom_FailsValidation()
    {
        var marque = new Marque { Nom = "" };

        var results = Validate(marque);

        Assert.NotEmpty(results);
    }

    private static IList<ValidationResult> Validate(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        return results;
    }
}
