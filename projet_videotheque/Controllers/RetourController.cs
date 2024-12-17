using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using videotheque.Models;
using videotheque.Data;

namespace videotheque.Controllers
{
    public class RetourController : Controller
    {
        private readonly AppDbContext _context;

        public RetourController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Retour(string? statut = null)
        {
            var query = _context.Locations
                .Include(l => l.User)
                .Include(l => l.LocationDetails)
                    .ThenInclude(ld => ld.Film)
                .Include(l => l.LocationDetails)
                    .ThenInclude(ld => ld.ExemplaireDVD)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statut))
            {
                query = query.Where(l => l.Statut == statut);
            }

            var locations = await query.ToListAsync();
            ViewBag.StatutActuel = statut;

            return View(locations);
        }

        [HttpPost]
        public async Task<IActionResult> TraiterRetour(int locationDetailId)
        {
            var locationDetail = await _context.LocationDetails
                .Include(ld => ld.Location)
                .Include(ld => ld.ExemplaireDVD)
                .FirstOrDefaultAsync(ld => ld.Id == locationDetailId);

            if (locationDetail == null)
                return NotFound();

            locationDetail.ExemplaireDVD.EstDansStock = true;
            locationDetail.Location.DateRetourEffective = DateTime.Now;

            var tousLesDetailsRetournes = await _context.LocationDetails
                .Where(ld => ld.LocationId == locationDetail.LocationId)
                .AllAsync(ld => ld.ExemplaireDVD.EstDansStock);

            if (tousLesDetailsRetournes)
            {
                locationDetail.Location.Statut = "Terminée";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Retour));
        }

        [HttpPost]
        public async Task<IActionResult> ChangerEtatStock(int exemplaireDvdId)
        {
            var exemplaire = await _context.ExemplairesDVD.FindAsync(exemplaireDvdId);

            if (exemplaire == null)
                return NotFound();

            exemplaire.EstDansStock = !exemplaire.EstDansStock;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Retour));
        }

        [HttpGet]
        public async Task<IActionResult> GetDVDInfo(string codeBarres)
        {
            var dvd = await _context.ExemplairesDVD
                .Include(d => d.Film)
                .FirstOrDefaultAsync(d => d.CodeBarre == codeBarres);

            if (dvd == null)
                return NotFound();

            return Json(new
            {
                exemplaireDvdId = dvd.Id,
                filmTitle = dvd.Film.Titre,
                estDansStock = dvd.EstDansStock
            });
        }
    }
}