using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using videotheque.Data;
using videotheque.ViewModels;

namespace videotheque.Controllers
{
public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Dashboard()
    {
        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);

        var viewModel = new DashboardViewModel
        {
            // Statistiques générales
            TotalFilms = await _context.Films.CountAsync(),
            TotalUsers = await _context.Users.CountAsync(),
            TotalLocationsEnCours = await _context.Locations
                .Where(l => l.DateRetourEffective == null)
                .CountAsync(),
            ChiffreAffairesMois = await _context.Locations
                .Where(l => l.DateLocation >= monthStart)
                .SumAsync(l => l.MontantTotal),

            // Prix moyens
            PrixMoyen24h = await _context.Films.AverageAsync(f => f.Prix24h),
            PrixMoyen48h = await _context.Films.AverageAsync(f => f.Prix48h),
            PrixMoyen72h = await _context.Films.AverageAsync(f => f.Prix72h),
            PrixMoyen1Semaine = await _context.Films.AverageAsync(f => f.Prix1Semaine),

            // 1. Dernières locations
            DernieresLocations = await _context.Locations
                .Include(l => l.User)
                .Include(l => l.LocationDetails)
                .ThenInclude(ld => ld.Film)
                .OrderByDescending(l => l.DateLocation)
                .Take(10)
                .Select(l => new LocationRecente
                {
                    UserName = l.User.FullName,
                    FilmTitle = l.LocationDetails.First().Film.Titre,
                    DateLocation = l.DateLocation,
                    DateRetourPrevue = l.DateRetourPrevue,
                    MontantTotal = l.MontantTotal
                })
                .ToListAsync(),

            // 2. Films les plus loués
            TopFilmsLoues = await _context.Films
                .Include(f => f.Locations)
                .Select(f => new FilmPopulaire
                {
                    Titre = f.Titre,
                    NombreLocations = f.Locations.Count(),
                    ScoreMoyen = f.ScoreMoyen,
                    ExemplairesDisponibles = f.ExemplairesDisponibles
                })
                .OrderByDescending(f => f.NombreLocations)
                .Take(10)
                .ToListAsync(),

            // 3. Films les mieux notés
            TopFilmsNotes = await _context.Films
                .OrderByDescending(f => f.ScoreMoyen)
                .Take(10)
                .Select(f => new FilmPopulaire
                {
                    Titre = f.Titre,
                    ScoreMoyen = f.ScoreMoyen,
                    ExemplairesDisponibles = f.ExemplairesDisponibles
                })
                .ToListAsync(),

            // 4. Locations en retard
            LocationsEnRetard = await _context.Locations
                .Include(l => l.User)
                .Include(l => l.LocationDetails)
                .ThenInclude(ld => ld.Film)
                .Where(l => l.DateRetourPrevue < today && l.DateRetourEffective == null)
                .Select(l => new LocationRetard
                {
                    UserName = l.User.FullName,
                    FilmTitle = l.LocationDetails.First().Film.Titre,
                    DateRetourPrevue = l.DateRetourPrevue,
                    JoursRetard = (int)(today - l.DateRetourPrevue).TotalDays
                })
                .ToListAsync(),

            // 5. Films en stock bas
            FilmsStockBas = await _context.Films
                .Where(f => f.ExemplairesDisponibles < 3)
                .OrderBy(f => f.ExemplairesDisponibles)
                .Take(10)
                .ToListAsync(),

            // 6. Tous les utilisateurs
            TousLesUtilisateurs = await _context.Users
                .OrderBy(u => u.FullName)
                .Select(u => new UserBasic
                {
                    FullName = u.FullName,
                    Email = u.Email,
                    Adresse = u.Adresse,
                    DateInscription = u.DateInscription
                })
                .ToListAsync(),

            // 7. Nouveaux utilisateurs
            NouveauxUtilisateurs = await _context.Users
                .Where(u => u.DateInscription >= monthStart)
                .OrderByDescending(u => u.DateInscription)
                .Select(u => new UserNouveau
                {
                    FullName = u.FullName,
                    DateInscription = u.DateInscription,
                    NombreLocations = u.Locations.Count()
                })
                .Take(10)
                .ToListAsync(),

            // 8. DVDs disponibles
            DVDsDisponibles = await _context.ExemplairesDVD
                .Include(d => d.Film)
                .Where(d => d.EstDansStock)
                .Select(d => new DVDDisponible
                {
                    FilmTitle = d.Film.Titre,
                    CodeBarre = d.CodeBarre,
                    ExemplairesDisponibles = d.Film.ExemplairesDisponibles
                })
                .Take(10)
                .ToListAsync()
        };

        return View(viewModel);
    }
}

}