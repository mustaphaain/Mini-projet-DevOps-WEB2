using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Locatic.Data;
using Locatic.Models;

namespace Locatic.Controllers;

public class ClientsController : Controller
{
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var clients = _context.Clients.ToList();
        return View(clients);
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
            return NotFound();

        var client = _context.Clients
            .Include(c => c.Reservations)
            .ThenInclude(r => r.Voiture)
            .FirstOrDefault(c => c.Id == id);

        return client == null ? NotFound() : View(client);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Client client)
    {
        if (ModelState.IsValid)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(client);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var client = _context.Clients.Find(id);
        return client == null ? NotFound() : View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Client client)
    {
        if (id != client.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(client);

        _context.Entry(client).State = EntityState.Modified;
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var client = _context.Clients.Find(id);
        return client == null ? NotFound() : View(client);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var client = _context.Clients.Find(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }
}