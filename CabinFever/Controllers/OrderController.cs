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

        public OrderController(ItemDbContext itemDbContext)
        {
            _itemDbContext = itemDbContext;
        }

        public async Task<IActionResult> Table()
        {
            List<Order> orders = await _itemDbContext.Orders.ToListAsync();
            return View(orders);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var orders = await _itemDbContext.Orders.ToListAsync();
            var createOrderItemViewModel = new CreateOrderItemViewModel
            {
                // Adjust the ViewModel to your needs, removing references to OrderItem
            };
            return View(createOrderItemViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(Item item, DateTime FromDate, DateTime ToDate, int guests, decimal totalPrice)
        {
            try
            {
                // Calculate TotalPrice based on your logic (e.g., item.PricePerNight * numberOfNights)
                // decimal totalPrice = CalculateTotalPrice(item, FromDate, ToDate, guests);

                var order = new Order
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    OrderDate = DateTime.Now.ToString(),
                    TotalPrice = totalPrice,
                    ItemId = item.Id
                };

                _itemDbContext.Orders.Add(order);
                await _itemDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Table));
            }
            catch
            {
                return BadRequest("Order creation failed.");
            }
        }
    }
}
