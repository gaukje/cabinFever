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
        public IActionResult CreateOrder(int itemId)
        {
            // Retrieve the item based on the provided itemId
            var item = _itemDbContext.Items.Find(itemId);

            if (item == null)
            {
                return NotFound("Item not found");
            }

            return View(new CreateOrderViewModel
            {
                ItemId = item.Id,
                ItemName = item.Name,
                MinCheckInDate = DateTime.Now.ToString("yyyy-MM-dd"),
                MaxCheckOutDate = DateTime.Now.AddMonths(6).ToString("yyyy-MM-dd"),
                MaxGuests = item.Capacity
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(int itemId, DateTime fromDate, DateTime toDate, int guests)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state");
                }

                var item = await _itemDbContext.Items.FindAsync(itemId);

                if (item == null)
                {
                    return NotFound("Item not found");
                }
                
                // Calculate the total number of nights
                int numberOfNights = (int)(toDate - fromDate).TotalDays;

                // Use your own logic to calculate total price based on item, guests, and numberOfNights
                decimal totalPrice = CalculateTotalPrice(item, guests, numberOfNights);

                var order = new Order
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    OrderDate = DateTime.Now.ToString(),
                    TotalPrice = totalPrice,
                    ItemId = item.Id,
                    FromDate = fromDate,
                    ToDate = toDate,
                };

                _itemDbContext.Orders.Add(order);
                await _itemDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Table));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                return BadRequest("Order creation failed.");
            }
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
