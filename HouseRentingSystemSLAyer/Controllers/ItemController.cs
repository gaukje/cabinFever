using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystemSLAyer.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
