using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using videotheque.Services;

namespace videotheque.Controllers
{
    public class PaiementController : Controller
    {
        private readonly IShoppingCartService _cartService;

        public PaiementController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Paiement(int locationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _cartService.GetUserCart(userId);
            ViewData["LocationId"] = locationId;
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                // Créer la location au moment du paiement
                var location = await _cartService.ConvertCartToLocation(userId);

                // Ici on simule un paiement réussi


                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Paiement");
            }
        }
    }
}