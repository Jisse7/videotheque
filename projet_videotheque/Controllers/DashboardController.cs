using Microsoft.AspNetCore.Mvc;

namespace videotheque.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
