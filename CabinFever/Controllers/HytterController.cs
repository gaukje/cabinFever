using Microsoft.AspNetCore.Mvc;

namespace CabinFever.Controllers
{
    public class HytterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
