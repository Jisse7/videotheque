using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using videotheque.Data;
using videotheque.Models;
using videotheque.ViewModels;

namespace videotheque.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index(string sortOrder)
        {
            // Récupère la liste des films avec leurs catégories
            var filmsQuery = _context.Films.Include(f => f.Categorie);

            // Applique le tri en fonction du paramètre sortOrder
            var films = sortOrder switch
            {
                "date" => filmsQuery.OrderByDescending(f => f.AnneeSortie).AsQueryable(),
                "genre" => filmsQuery.OrderBy(f => f.Categorie.Nom).AsQueryable(),
                "duree" => filmsQuery.OrderByDescending(f => f.DureeMinutes).AsQueryable(),
                "score" => filmsQuery.OrderByDescending(f => f.ScoreMoyen).AsQueryable(),
                "touslesfilms" or null => filmsQuery.OrderBy(f => f.Titre).AsQueryable(),
                _ => filmsQuery.OrderBy(f => f.Titre).AsQueryable()
            };

            var viewModel = new MovieGridViewModel
            {
                Films = await films.ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };

            // Stocker la valeur du tri actuel pour l'interface
            ViewBag.CurrentSort = sortOrder;

            return View(viewModel);
        }
        /*    public async Task<IActionResult> Index()
            {
                var viewModel = new MovieGridViewModel
                {
                    Films = await _context.Films.Include(f => f.Categorie).ToListAsync(),
                    Categories = await _context.Categories.ToListAsync()
                };
                return View(viewModel);
            }*/

        /*  public IActionResult Index()
          {
              return View();
          }*/

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}