using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using videotheque.Models;
using videotheque.Services;

[Authorize] // Seuls les utilisateurs connectés peuvent accéder au panier
public class CartController : Controller
{
    private readonly IShoppingCartService _cartService;
    public CartController(IShoppingCartService cartService)
    {
        _cartService = cartService;
    }

    // Affiche le panier
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cartItems = await _cartService.GetUserCart(userId);
        return View(cartItems);
    }

    // Ajoute un film au panier
    [HttpPost]
    public async Task<IActionResult> AddToCart(int filmId, int dureeLocation)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            await _cartService.AddToCart(filmId, userId, dureeLocation);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Details", "Films", new { id = filmId });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GoToPayment()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            var cartItems = await _cartService.GetUserCart(userId);
            if (!cartItems.Any())
            {
                throw new Exception("Votre panier est vide");
            }
            return RedirectToAction("Paiement", "Paiement");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            await _cartService.RemoveFromCart(cartItemId);
            /*return RedirectToAction(nameof(Index));*/
            return Redirect(Request.Headers["Referer"].ToString());
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            /*return RedirectToAction(nameof(Index));*/
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

   /* [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int cartItemId, string returnUrl)
    {
        try
        {
            await _cartService.RemoveFromCart(cartItemId);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }*/
}