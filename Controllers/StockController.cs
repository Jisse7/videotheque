using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using videotheque.Data;
using videotheque.Models;
using videotheque.ViewModels;

//CRUD POUR STOCK
//-SUPPRIMER un film marche si il n'est PAS dans le panier 

namespace videotheque.Controllers
{
    public class StockController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StockController> _logger;

        public StockController(AppDbContext context, ILogger<StockController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var films = await _context.Films
                    .Include(f => f.Categorie)
                    .Select(f => new StockViewModel
                    {
                        Id = f.Id,
                        Titre = f.Titre,
                        Realisateur = f.Realisateur,
                        AnneeSortie = f.AnneeSortie,
                        Synopsis = f.Synopsis,
                        ImageUrl = f.ImageUrl,
                        TrailerUrl = f.TrailerUrl,
                        NombreExemplaires = f.NombreExemplaires,
                        ExemplairesDisponibles = f.ExemplairesDisponibles,
                        ScoreMoyen = f.ScoreMoyen,
                        DureeMinutes = f.DureeMinutes,
                        Prix24h = f.Prix24h,
                        Prix48h = f.Prix48h,
                        Prix72h = f.Prix72h,
                        Prix1Semaine = f.Prix1Semaine,
                        CategorieId = f.CategorieId,
                        CategorieName = f.Categorie.Nom
                    })
                    .ToListAsync();

                return View(films);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des films");
                return Problem("Une erreur est survenue lors de la récupération des films.");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(new CreateFilmViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du formulaire de création");
                return Problem("Une erreur est survenue lors du chargement du formulaire.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFilmViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var film = new Film
                    {
                        Titre = viewModel.Titre,
                        Realisateur = viewModel.Realisateur,
                        AnneeSortie = viewModel.AnneeSortie,
                        Synopsis = viewModel.Synopsis,
                        ImageUrl = viewModel.ImageUrl,
                        TrailerUrl = viewModel.TrailerUrl,
                        NombreExemplaires = viewModel.NombreExemplaires,
                        ExemplairesDisponibles = viewModel.NombreExemplaires,
                        ScoreMoyen = 0,
                        DureeMinutes = viewModel.DureeMinutes,
                        Prix24h = viewModel.Prix24h,
                        Prix48h = viewModel.Prix48h,
                        Prix72h = viewModel.Prix72h,
                        Prix1Semaine = viewModel.Prix1Semaine,
                        CategorieId = viewModel.CategorieId
                    };

                    _context.Films.Add(film);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la création du film");
                    ModelState.AddModelError("", "Une erreur est survenue lors de la création du film.");
                }
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var film = await _context.Films
                    .Include(f => f.Categorie)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (film == null)
                {
                    return NotFound();
                }

                ViewBag.Categories = await _context.Categories.ToListAsync();

                var viewModel = new StockViewModel
                {
                    Id = film.Id,
                    Titre = film.Titre,
                    Realisateur = film.Realisateur,
                    AnneeSortie = film.AnneeSortie,
                    Synopsis = film.Synopsis,
                    ImageUrl = film.ImageUrl,
                    TrailerUrl = film.TrailerUrl,
                    NombreExemplaires = film.NombreExemplaires,
                    ExemplairesDisponibles = film.ExemplairesDisponibles,
                    ScoreMoyen = film.ScoreMoyen,
                    DureeMinutes = film.DureeMinutes,
                    Prix24h = film.Prix24h,
                    Prix48h = film.Prix48h,
                    Prix72h = film.Prix72h,
                    Prix1Semaine = film.Prix1Semaine,
                    CategorieId = film.CategorieId,
                    CategorieName = film.Categorie?.Nom
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération du film {id}");
                return Problem("Une erreur est survenue lors de la récupération du film.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StockViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var film = await _context.Films.FindAsync(id);
                    if (film == null)
                    {
                        return NotFound();
                    }

                    film.Titre = model.Titre;
                    film.Realisateur = model.Realisateur;
                    film.AnneeSortie = model.AnneeSortie;
                    film.Synopsis = model.Synopsis;
                    film.ImageUrl = model.ImageUrl;
                    film.TrailerUrl = model.TrailerUrl;
                    film.DureeMinutes = model.DureeMinutes;
                    film.Prix24h = model.Prix24h;
                    film.Prix48h = model.Prix48h;
                    film.Prix72h = model.Prix72h;
                    film.Prix1Semaine = model.Prix1Semaine;
                    film.CategorieId = model.CategorieId;

                    _context.Update(film);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!FilmExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"Erreur de concurrence lors de la mise à jour du film {id}");
                    ModelState.AddModelError("", "Une erreur de concurrence est survenue. Veuillez réessayer.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la mise à jour du film {id}");
                ModelState.AddModelError("", "Une erreur est survenue lors de la mise à jour.");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var film = await _context.Films.FindAsync(id);
                if (film == null)
                {
                    return NotFound();
                }

                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la suppression du film {id}");
                return Problem("Une erreur est survenue lors de la suppression du film.");
            }
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }
    }
}