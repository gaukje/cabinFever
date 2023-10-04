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
        public async Task<IActionResult> CreateOrderItem()
        {
            var items = await _itemDbContext.Items.ToListAsync();
            var orders = await _itemDbContext.Orders.ToListAsync();
            var createOrderItemViewModel = new CreateOrderItemViewModel
            {
                // Adjust the ViewModel to your needs, removing references to OrderItem
            };
            return View(createOrderItemViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderItem orderItem)
        {
            try
            {
                var newItem = _itemDbContext.Items.Find(orderItem.ItemId);
                var newOrder = _itemDbContext.Orders.Find(orderItem.OrderId);

                if (newItem == null || newOrder == null)
                {
                    return BadRequest("Item or Order not found.");
                }

                var newOrderItem = new OrderItem
                {
                    ItemId = orderItem.ItemId,
                    Item = newItem,
                    AmountNights = orderItem.AmountNights,
                    OrderId = orderItem.OrderId,
                    Order = newOrder,
                };
                newOrderItem.OrderItemPrice = orderItem.AmountNights * newOrderItem.Item.PricePerNight;

                _itemDbContext.OrderItems.Add(newOrderItem);
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
