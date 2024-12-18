using Microsoft.EntityFrameworkCore;
using videotheque.Data;

namespace videotheque.Services
{
    public class RetourService : IRetourService
    {
        private readonly AppDbContext _context;
        private readonly IDVDManagementService _dvdService;

        public RetourService(AppDbContext context, IDVDManagementService dvdService)
        {
            _context = context;
            _dvdService = dvdService;
        }

        public async Task<LocationDetail> GetLocationDetailByDVDId(int dvdId)
        {
            return await _context.LocationDetails
                .Include(ld => ld.Location)
                .Include(ld => ld.Film)
                .Include(ld => ld.ExemplaireDVD)
                .FirstOrDefaultAsync(ld => ld.ExemplaireDVDId == dvdId &&
                                         ld.Location.DateRetourEffective == null);
        }

        public async Task<(bool success, string message)> ValidateReturn(int dvdId)
        {
            var dvd = await _context.ExemplairesDVD.FindAsync(dvdId);
            if (dvd == null)
                return (false, "DVD non trouvé.");

            if (dvd.EstDansStock)
                return (false, "Ce DVD est déjà en stock.");

            var locationDetail = await GetLocationDetailByDVDId(dvdId);
            if (locationDetail == null)
                return (false, "Aucune location active trouvée pour ce DVD.");

            return (true, "DVD valide pour le retour.");
        }

        public async Task<bool> ProcessDVDReturn(int dvdId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var validation = await ValidateReturn(dvdId);
                if (!validation.success)
                    return false;

                var locationDetail = await GetLocationDetailByDVDId(dvdId);
                var location = locationDetail.Location;

                // Marquer le DVD comme retourné
                await _dvdService.MarkDVDAsReturned(dvdId);

                // Vérifier si tous les DVD de la location sont retournés
                var allDVDsReturned = await CheckAllDVDsReturned(location.Id);
                if (allDVDsReturned)
                {
                    location.DateRetourEffective = DateTime.UtcNow;
                    location.Statut = "Terminée";
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        private async Task<bool> CheckAllDVDsReturned(int locationId)
        {
            var locationDetails = await _context.LocationDetails
                .Include(ld => ld.ExemplaireDVD)
                .Where(ld => ld.LocationId == locationId)
                .ToListAsync();

            return locationDetails.All(ld => ld.ExemplaireDVD.EstDansStock);
        }
    }
}
