using CabinFever.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Remember to add this import for ILogger
using System.Diagnostics;
using System.Threading.Tasks; // Remember to add this import for Task
using CabinFever.DAL; // Remember to add this import for ItemRepository
using CabinFever.ViewModels; // Remember to add this import for ItemListViewModel
using System.Linq; // Remember to add this import for Enumerable.Empty
using System.Security.Claims;

namespace CabinFever.Controllers
{
    public class HomeController : Controller //inherit from Controller class
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IItemRepository _itemRepository;

        //Constructor that accepts ILogger and IItemRepository through dependency injection
        public HomeController(ILogger<HomeController> logger, IItemRepository itemRepository)
        {
            _logger = logger;
            _itemRepository = itemRepository;
        }

        //Index action method & makes it async
        public async Task<IActionResult> Index()
        {
            // retrive all items from the database asynchronously
            var items = await _itemRepository.GetAll();

            // retrive all items from the database asynchronously
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //retrive orders for the user asynchronously
            var orders = await _itemRepository.GetOrdersForUser(userId);

            //create an ItemListViewModel and send it to the view
            var model = new ItemListViewModel(items ?? Enumerable.Empty<Item>(), "Rentals", orders);// Send items to the view

            return View(model);
        }

        // Rentals action method & makes it async
        public async Task<IActionResult> Rentals()
        {
            // retrive all items from the database asynchronously
            var items = await _itemRepository.GetAll();

            // retrive all items from the database asynchronously
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //retrive orders for the user asynchronously
            var orders = await _itemRepository.GetOrdersForUser(userId);

            //create an ItemListViewModel and send it to the view
            var model = new ItemListViewModel(items ?? Enumerable.Empty<Item>(), "Rentals", orders);// Send items to the view

            return View(model);
        }

        //Definerer action method
        public IActionResult About()
        {
            //returnerer the 'about' view
            return View();
        }

        //contact action method
        public IActionResult Contact()
        {
            //return contact view
            return View();
        }

        //define MinSide action method & makes this method async
        public async Task<IActionResult> MinSide() 
        {
            //same as the index and rentals method, it gets the items from the database
            var items = await _itemRepository.GetAll(); 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _itemRepository.GetOrdersForUser(userId);

            var model = new ItemListViewModel(items ?? Enumerable.Empty<Item>(), "MinSide", orders); // Send items til visningen
            return View(model);
        }

        //Define the error action method for handling errors
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //return the Error view with an ErrorViewModel
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
