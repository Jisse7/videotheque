using Microsoft.EntityFrameworkCore;
using videotheque.Data;
using videotheque.Models;

namespace videotheque.Services
{
    public interface IDVDManagementService
    {
        Task<ExemplaireDVD> GetAvailableDVDForFilm(int filmId);
        Task<bool> MarkDVDAsRented(int dvdId, int locationDetailId);
        Task<bool> MarkDVDAsReturned(int dvdId);
        Task<IEnumerable<ExemplaireDVD>> GetAllDVDsForFilm(int filmId);
    }

    public class DVDManagementService : IDVDManagementService
    {
        private readonly AppDbContext _context;
        private readonly Random _random;

        public DVDManagementService(AppDbContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task<ExemplaireDVD> GetAvailableDVDForFilm(int filmId)
        {
            // Récupérer tous les DVD disponibles pour ce film
            var availableDVDs = await _context.ExemplairesDVD
                .Where(d => d.FilmId == filmId && d.EstDansStock)
                .ToListAsync();

            if (!availableDVDs.Any())
            {
                throw new InvalidOperationException("Aucun DVD disponible pour ce film.");
            }

            // Sélectionner un DVD aléatoirement parmi ceux disponibles
            int randomIndex = _random.Next(availableDVDs.Count);
            return availableDVDs[randomIndex];
        }

        public async Task<bool> MarkDVDAsRented(int dvdId, int locationDetailId)
        {
            var dvd = await _context.ExemplairesDVD.FindAsync(dvdId);
            if (dvd == null || !dvd.EstDansStock)
            {
                return false;
            }

            // Mettre à jour le statut du DVD
            dvd.EstDansStock = false;

            // Associer le DVD à la LocationDetail
            var locationDetail = await _context.LocationDetails.FindAsync(locationDetailId);
            if (locationDetail != null)
            {
                locationDetail.ExemplaireDVDId = dvdId;
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> MarkDVDAsReturned(int dvdId)
        {
            var dvd = await _context.ExemplairesDVD.FindAsync(dvdId);
            if (dvd == null)
            {
                return false;
            }

            dvd.EstDansStock = true;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ExemplaireDVD>> GetAllDVDsForFilm(int filmId)
        {
            return await _context.ExemplairesDVD
                .Where(d => d.FilmId == filmId)
                .ToListAsync();
        }
    }
}