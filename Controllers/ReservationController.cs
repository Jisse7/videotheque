using Microsoft.AspNetCore.Mvc;

namespace videotheque.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Reservation()
        {
            return View();
        }
    }
}
