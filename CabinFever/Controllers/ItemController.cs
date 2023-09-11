using Microsoft.AspNetCore.Mvc;

namespace CabinFever.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
