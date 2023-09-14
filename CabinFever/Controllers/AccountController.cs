using Microsoft.AspNetCore.Mvc;

namespace CabinFever.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult LoggInn()
        {
            return View();
        }

        public IActionResult Registrer()
        {
            return View();
        }
    }
}
