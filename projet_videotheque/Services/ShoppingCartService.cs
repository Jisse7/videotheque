using Microsoft.EntityFrameworkCore;
using videotheque.Data;
using videotheque.Models;

namespace videotheque.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _context;

        public ShoppingCartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> AddToCart(int filmId, string userId, int dureeLocation)
        {
            var film = await _context.Films
                .FirstOrDefaultAsync(f => f.Id == filmId && f.ExemplairesDisponibles > 0);

            if (film == null)
                throw new InvalidOperationException("Film non disponible à la location");

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.FilmId == filmId && ci.UserId == userId);

            if (existingItem != null)
                throw new InvalidOperationException("Ce film est déjà dans votre panier");

            // Déterminer le prix selon la durée
            decimal prix = dureeLocation switch
            {
                24 => film.Prix24h,
                48 => film.Prix48h,
                72 => film.Prix72h,
                168 => film.Prix1Semaine,
                _ => throw new InvalidOperationException("Durée de location invalide")
            };

            var cartItem = new CartItem
            {
                FilmId = filmId,
                UserId = userId,
                CartId = Guid.NewGuid().ToString(),
                DateAjout = DateTime.UtcNow,
                DureeLocation = dureeLocation,
                PrixLocation = prix
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return cartItem;
        }

        public async Task<bool> RemoveFromCart(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CartItem>> GetUserCart(string userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Film)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();
        }

        public async Task<Location> ConvertCartToLocation(string userId)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Film)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                throw new InvalidOperationException("Le panier est vide");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var location = new Location
                {
                    UserId = userId,
                    DateLocation = DateTime.UtcNow,
                    DateRetourPrevue = DateTime.UtcNow.AddHours(cartItems.Max(ci => ci.DureeLocation)),
                    Statut = "En cours",
                    MontantTotal = cartItems.Sum(ci => ci.PrixLocation)
                };

                _context.Locations.Add(location);
                await _context.SaveChangesAsync();

                foreach (var item in cartItems)
                {
                    // Rechercher un exemplaire DVD disponible
                    var exemplaireDVD = await _context.ExemplairesDVD
                        .Where(e => e.FilmId == item.FilmId && e.EstDansStock)
                        .FirstOrDefaultAsync();

                    if (exemplaireDVD == null)
                    {
                        throw new InvalidOperationException($"Aucun exemplaire disponible pour le film {item.Film.Titre}");
                    }

                    var detail = new LocationDetail
                    {
                        LocationId = location.Id,
                        FilmId = item.FilmId,
                        PrixUnitaire = item.PrixLocation,
                        DureeLocation = item.DureeLocation,
                        ExemplaireDVDId = exemplaireDVD.Id
                    };

                    _context.LocationDetails.Add(detail);

                    // On ne modifie plus l'état du DVD ici
                    // L'admin le fera manuellement quand le client viendra chercher le DVD
                    item.Film.ExemplairesDisponibles--;
                }

                // Créer le paiement
                var paiement = new Paiement
                {
                    LocationId = location.Id,
                    Montant = location.MontantTotal,
                    DatePaiement = DateTime.UtcNow,
                    MethodePaiement = "Carte bancaire",
                    StatutPaiement = "Payé"
                };

                _context.Paiements.Add(paiement);

                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return location;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}