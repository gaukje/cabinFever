using CabinFever.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Husk å legge til denne importen for ILogger
using System.Diagnostics;
using System.Threading.Tasks; // Husk å legge til denne importen for Task
using CabinFever.DAL; // Husk å legge til denne importen for ItemRepository
using CabinFever.ViewModels; // Husk å legge til denne importen for ItemListViewModel
using System.Linq; // Husk å legge til denne importen for Enumerable.Empty
using System.Security.Claims;

namespace CabinFever.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IItemRepository _itemRepository;

        public HomeController(ILogger<HomeController> logger, IItemRepository itemRepository)
        {
            _logger = logger;
            _itemRepository = itemRepository;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemRepository.GetAll(); // Hent alle items fra databasen
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _itemRepository.GetOrdersForUser(userId);

            var model = new ItemListViewModel(items ?? Enumerable.Empty<Item>(), "Rentals", orders);// Send items to the view

            return View(model);
        }

        public async Task<IActionResult> Rentals()
        {
            var items = await _itemRepository.GetAll(); // Hent alle items fra databasen
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _itemRepository.GetOrdersForUser(userId);

            var model = new ItemListViewModel(items ?? Enumerable.Empty<Item>(), "Rentals", orders);// Send items to the view

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> MinSide() // Gjør denne metoden asynkron
        {
            var items = await _itemRepository.GetAll(); // Hent alle items fra databasen
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _itemRepository.GetOrdersForUser(userId);

            var model = new ItemListViewModel(items ?? Enumerable.Empty<Item>(), "MinSide", orders); // Send items til visningen
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
