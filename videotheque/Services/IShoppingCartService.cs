using videotheque.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace videotheque.Services
{
    public interface IShoppingCartService
    {
        Task<CartItem> AddToCart(int filmId, string userId, int dureeLocation);
        Task<bool> RemoveFromCart(int cartItemId);
        Task<List<CartItem>> GetUserCart(string userId);
        Task<Location> ConvertCartToLocation(string userId);
    }
}