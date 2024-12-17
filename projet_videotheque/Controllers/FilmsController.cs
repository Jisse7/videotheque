using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using videotheque.Models;
using videotheque.Data;
using videotheque.ViewModels;

namespace videotheque.Controllers
{
    public class FilmsController : Controller
    {
        private readonly AppDbContext _context;

        public FilmsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Films/FilmDetails/5
        public async Task<IActionResult> FilmDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .Include(f => f.Categorie)
                .Include(f => f.Evaluations)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            // Vérifier la disponibilité
            bool isAvailable = film.ExemplairesDisponibles > 0;

            // Créer un viewModel pour la vue
            var viewModel = new FilmDetailsViewModel
            {
                Id = film.Id,
                Titre = film.Titre,
                Realisateur = film.Realisateur,
                AnneeSortie = film.AnneeSortie,
                Synopsis = film.Synopsis,
                ImageUrl = film.ImageUrl,
                TrailerUrl = film.TrailerUrl,
                ScoreMoyen = film.ScoreMoyen,
                DureeMinutes = film.DureeMinutes,
                EstDisponible = isAvailable,
                NombreExemplairesDisponibles = film.ExemplairesDisponibles,
                Prix24h = film.Prix24h,
                Prix48h = film.Prix48h,
                Prix72h = film.Prix72h,
                Prix1Semaine = film.Prix1Semaine,
                CategorieNom = film.Categorie?.Nom
            };

            return View(viewModel);
        }
    }
}