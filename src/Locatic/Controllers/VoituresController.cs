using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Locatic.Data;
using Locatic.Models;

namespace Locatic.Controllers;

public class VoituresController : Controller
{
    private readonly AppDbContext _context;

    public VoituresController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var voitures = _context.Voitures
            .Include(v => v.Modele)
                .ThenInclude(m => m.Marque)
            .ToList();

        return View(voitures);
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
            return NotFound();

        var voiture = _context.Voitures
            .Include(v => v.Modele)
                .ThenInclude(m => m.Marque)
            .FirstOrDefault(v => v.Id == id);

        return voiture == null ? NotFound() : View(voiture);
    }

    public IActionResult Create()
    {
        PopulateModeles();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Voiture voiture)
    {
        if (ModelState.IsValid)
        {
            _context.Voitures.Add(voiture);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        PopulateModeles(voiture.ModeleId);
        return View(voiture);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var voiture = _context.Voitures.Find(id);
        if (voiture == null)
            return NotFound();

        PopulateModeles(voiture.ModeleId);
        return View(voiture);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Voiture voiture)
    {
        if (id != voiture.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            PopulateModeles(voiture.ModeleId);
            return View(voiture);
        }

        _context.Entry(voiture).State = EntityState.Modified;
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var voiture = _context.Voitures
            .Include(v => v.Modele)
                .ThenInclude(m => m.Marque)
            .FirstOrDefault(v => v.Id == id);

        return voiture == null ? NotFound() : View(voiture);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var voiture = _context.Voitures.Find(id);
        if (voiture != null)
        {
            _context.Voitures.Remove(voiture);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateModeles(object selectedModel = null)
    {
        var modeles = _context.Modeles
            .Include(m => m.Marque)
            .Select(m => new
            {
                m.Id,
                Label = m.Marque != null ? $"{m.Marque.Nom} / {m.Nom}" : m.Nom
            })
            .ToList();

        ViewBag.Modeles = new SelectList(modeles, "Id", "Label", selectedModel);
    }
}