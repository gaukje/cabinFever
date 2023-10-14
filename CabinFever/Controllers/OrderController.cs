using Microsoft.AspNetCore.Mvc;
using CabinFever.Models;
using Microsoft.EntityFrameworkCore;
using CabinFever.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using CabinFever.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;

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
            if (!User.Identity.IsAuthenticated)
            {
                // User is not authenticated, handle it accordingly
                // You can redirect back to the reservation page or display an error message
                // For example, you can add a ModelState error and return to the view:
                ModelState.AddModelError(string.Empty, "You must be logged in to create an order.");
                return View(order);
            }

            // User is already logged in
            if (ModelState.IsValid)
            {
                // Your order creation logic here
                try
                {
                    _itemDbContext.Orders.Add(order);
                    await _itemDbContext.SaveChangesAsync();
                    return RedirectToAction("Index", "Home"); // Redirect to a success page
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while saving order: {@Order}", order);
                    // Handle the exception more gracefully, e.g., by displaying an error page to the user.
                    return RedirectToAction("Error", "Home");
                }
            }

            // If ModelState is not valid, return to the Create view to display validation errors
            return View(order);
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

