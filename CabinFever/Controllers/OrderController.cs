using Microsoft.AspNetCore.Mvc;
using CabinFever.Models;
using Microsoft.EntityFrameworkCore;
using CabinFever.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using CabinFever.DAL;
using Microsoft.AspNetCore.Authorization;

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
            var items = await _itemDbContext.Items.ToListAsync();
            var orders = await _itemDbContext.Orders.ToListAsync();
            var createOrderViewModel = new CreateOrderViewModel
            {
                // Adjust the ViewModel to your needs, removing references to OrderItem
            };
            return View(createOrderViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            try
            {
                // Adjust the logic to create Order and associate Items to it
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
