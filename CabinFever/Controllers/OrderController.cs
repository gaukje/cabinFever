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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Order order)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if not authenticated
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                order.UserId = userId;

                _logger.LogInformation("Order before saving: {@Order}", order);

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

                    return View(order);
                }

                try
                {
                    _itemDbContext.Orders.Add(order);
                    await _itemDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while saving order: {@Order}", order);
                    throw;
                }

                return RedirectToAction("Index", "Home");
            }

            // In case something unexpected happens
            return RedirectToAction("Error", "Home");
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

