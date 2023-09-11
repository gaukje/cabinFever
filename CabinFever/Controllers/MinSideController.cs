using Microsoft.AspNetCore.Mvc;

namespace CabinFever.Controllers
{
    public class OmossController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
