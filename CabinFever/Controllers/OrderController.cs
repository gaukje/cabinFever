using Microsoft.AspNetCore.Mvc;
using CabinFever.Models;
using Microsoft.EntityFrameworkCore;
using CabinFever.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using CabinFever.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CabinFever.Controllers
{
    public class OrderController : Controller
    {
        private readonly ItemDbContext _itemDbContext;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ItemDbContext itemDbContext, ILogger<OrderController> logger)
        {
            _itemDbContext = itemDbContext;
            _logger = logger;
        }

        public async Task<IActionResult> Table()
        {
            List<Order> orders = await _itemDbContext.Orders.ToListAsync();
            return View(orders);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            // Hent UserId
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Sjekk om UserId er gyldig
            if (userId == null)
            {
                // Logg en feil for å hjelpe med feilsøking
                _logger.LogError("UserId is null. User may not be logged in, or ClaimTypes.NameIdentifier may not be set.");

                // Håndter feil (f.eks. bruker ikke logget inn)
                return Unauthorized(); // Eller en annen passende respons
            }

            // Sett UserId på order
            order.UserId = userId;

            // Oppdater ModelState manuelt
            ModelState.Clear();
            TryValidateModel(order);

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError("Model validation error for {Key}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }

                // Return the view with the model to display validation error messages
                return View(order);
            }

            // Your logic to save the order goes here
            // ...

            // Redirect to another action as per your flow
            return RedirectToAction("Index", "Home");
        }



        private decimal CalculateTotalPrice(Item item, int guests, int numberOfNights)
        {
            decimal pricePerNight = item.PricePerNight;
            decimal cleaningFee = 400 + (pricePerNight * 0.05m);
            decimal serviceFee = pricePerNight * 0.10m;
            decimal taxes = pricePerNight * 0.025m;

            decimal totalPrice = (pricePerNight * numberOfNights) + cleaningFee + serviceFee + taxes;

            return totalPrice;
        }
    }
}

