using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using videotheque.Services;
using videotheque.Models;
using Microsoft.AspNetCore.Authorization;
using videotheque.Data;

namespace videotheque.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDVDManagementService _dvdService;
        private readonly IShoppingCartService _cartService;

        public LocationController(
            AppDbContext context,
            IDVDManagementService dvdService,
            IShoppingCartService cartService)
        {
            _context = context;
            _dvdService = dvdService;
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessLocation(int locationDetailId)
        {
            try
            {
                // Récupérer le détail de la location
                var locationDetail = await _context.LocationDetails
                    .Include(ld => ld.Film)
                    .FirstOrDefaultAsync(ld => ld.Id == locationDetailId);

                if (locationDetail == null)
                {
                    return NotFound("Détail de location non trouvé.");
                }

                // Obtenir un DVD disponible pour ce film
                var dvd = await _dvdService.GetAvailableDVDForFilm(locationDetail.FilmId);

                // Marquer le DVD comme loué
                var success = await _dvdService.MarkDVDAsRented(dvd.Id, locationDetailId);

                if (!success)
                {
                    throw new Exception("Impossible de marquer le DVD comme loué.");
                }

                // Mettre à jour le nombre d'exemplaires disponibles du film
                locationDetail.Film.ExemplairesDisponibles--;
                await _context.SaveChangesAsync();

                TempData["Success"] = $"DVD attribué avec succès. Code-barres : {dvd.CodeBarre}";
                return RedirectToAction("ConfirmationLocation", new { locationId = locationDetail.LocationId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de l'attribution du DVD : {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Location()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var locations = await _context.Locations
                .Include(l => l.LocationDetails)
                    .ThenInclude(ld => ld.Film)
                .Include(l => l.LocationDetails)
                    .ThenInclude(ld => ld.ExemplaireDVD)  // Ajout de cette ligne
                .Include(l => l.Paiement)
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.DateLocation)
                .ToListAsync();

            return View(locations);
        }

        [HttpPost]
        public async Task<IActionResult> RetournerDVD(int dvdId)
        {
            try
            {
                var success = await _dvdService.MarkDVDAsReturned(dvdId);
                if (!success)
                {
                    throw new Exception("Impossible de marquer le DVD comme retourné.");
                }

                // Mettre à jour le nombre d'exemplaires disponibles du film
                var dvd = await _context.ExemplairesDVD
                    .Include(d => d.Film)
                    .FirstOrDefaultAsync(d => d.Id == dvdId);

                if (dvd != null)
                {
                    dvd.Film.ExemplairesDisponibles++;
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "DVD retourné avec succès.";
                return RedirectToAction("Location");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors du retour du DVD : {ex.Message}";
                return RedirectToAction("Location");
            }
        }
    }



}