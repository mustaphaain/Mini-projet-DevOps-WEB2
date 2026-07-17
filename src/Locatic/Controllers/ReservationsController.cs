using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Locatic.Data;
using Locatic.Models;

namespace Locatic.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var reservations = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Voiture)
                    .ThenInclude(v => v.Modele)
                        .ThenInclude(m => m.Marque)
                .ToList();

            return View(reservations);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var reservation = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Voiture)
                    .ThenInclude(v => v.Modele)
                        .ThenInclude(m => m.Marque)
                .FirstOrDefault(r => r.Id == id);

            return reservation == null ? NotFound() : View(reservation);
        }

        public IActionResult Create()
        {
            PopulateSelections();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reservation reservation)
        {
            PopulateSelections();

            if (!ModelState.IsValid)
                return View(reservation);

            if (!ValidateReservation(reservation, out var voiture))
                return View(reservation);

            if (IsVoitureReserved(reservation.VoitureId, reservation.DateDebut, reservation.DateFin))
            {
                ModelState.AddModelError("", "Cette voiture est déjà réservée sur la période choisie.");
                return View(reservation);
            }

            var jours = (reservation.DateFin - reservation.DateDebut).Days;
            reservation.PrixTotal = jours * voiture.TarifJournalier;

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
                return NotFound();

            PopulateSelections(reservation.ClientId, reservation.VoitureId);
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Reservation reservation)
        {
            if (id != reservation.Id)
                return BadRequest();

            PopulateSelections(reservation.ClientId, reservation.VoitureId);

            if (!ModelState.IsValid)
                return View(reservation);

            if (!ValidateReservation(reservation, out var voiture))
                return View(reservation);

            if (IsVoitureReserved(reservation.VoitureId, reservation.DateDebut, reservation.DateFin, reservation.Id))
            {
                ModelState.AddModelError("", "Cette voiture est déjà réservée sur la période choisie.");
                return View(reservation);
            }

            var jours = (reservation.DateFin - reservation.DateDebut).Days;
            reservation.PrixTotal = jours * voiture.TarifJournalier;

            _context.Entry(reservation).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var reservation = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Voiture)
                .FirstOrDefault(r => r.Id == id);

            return reservation == null ? NotFound() : View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateSelections(int? selectedClient = null, int? selectedVoiture = null)
        {
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Nom", selectedClient);
            ViewBag.Voitures = new SelectList(_context.Voitures.Select(v => new
            {
                v.Id,
                Label = v.Immatriculation + " (" + v.Modele!.Nom + ")"
            }), "Id", "Label", selectedVoiture);
        }

        private bool ValidateReservation(Reservation reservation, out Voiture voiture)
        {
            voiture = _context.Voitures.Find(reservation.VoitureId);
            if (voiture == null)
            {
                ModelState.AddModelError("", "Voiture introuvable.");
                return false;
            }

            if (reservation.DateFin <= reservation.DateDebut)
            {
                ModelState.AddModelError("", "La date de fin doit être strictement après la date de début.");
                return false;
            }

            return true;
        }

        private bool IsVoitureReserved(int voitureId, DateTime dateDebut, DateTime dateFin, int? excludeReservationId = null)
        {
            return _context.Reservations.Any(r => r.VoitureId == voitureId
                && r.Id != excludeReservationId
                && r.DateDebut < dateFin
                && dateDebut < r.DateFin);
        }
    }
}