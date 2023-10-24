using Microsoft.AspNetCore.Mvc;
using CabinFever.Models;
using Microsoft.EntityFrameworkCore;
using CabinFever.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using CabinFever.DAL;
using Microsoft.AspNetCore.Authorization; // Import the necessary namespace for authorization
using System.Security.Claims; // Import the necessary namespace for handling user claims
using Newtonsoft.Json; // Import Newtonsoft.Json for JSON handling

namespace CabinFever.Controllers
{
    public class OrderController : Controller
    {
        private readonly ItemDbContext _itemDbContext;  // Reference to the ItemDbContext
        private readonly ILogger<OrderController> _logger; // Logger for OrderController

        public OrderController(ItemDbContext itemDbContext, ILogger<OrderController> logger)
        {
            _itemDbContext = itemDbContext; // Initialize ItemDbContext
            _logger = logger; // Initialize logger
        }

        // An action method to display a table of orders
        public async Task<IActionResult> Table()
        {
            // Retrieving a list of orders from the database
            List<Order> orders = await _itemDbContext.Orders.ToListAsync();

            // Returning a view with the list of orders
            return View(orders);
        }

        // Get method to create a new order.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Action method to retriece date ranges for a specific item.
        [HttpGet]
        public IActionResult GetDateRange(int itemId)
        {
            // Retrieving date ranges from the database based on the item ID
            var dateRanges = _itemDbContext.Orders.Where(order => order.ItemId == itemId && order.ToDate >= DateTime.Today)
                .Select(order => new { order.FromDate, order.ToDate })
                .ToList();

            // Creating a list of date strings from the retrieved date ranges
            var dateList = new List<String>();

            foreach (var dateRange in dateRanges)
            {
                for (var date = dateRange.FromDate; date <= dateRange.ToDate; date = date.AddDays(1))
                {
                    var stringDate = date.ToString("yyyy-MM-dd");
                    dateList.Add(stringDate);
                }
            }

            // Returning the dateList as JSON
            return Json(dateList);
        }

        // Post method to create a new order.
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            // Retrieving the user's ID from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            order.UserId = userId;                                              // user's ID on the order      

            // Logging order state
            _logger.LogInformation("Order before saving: {@Order}", order);

            // Manually updating the ModelState
            ModelState.Clear();
            TryValidateModel(order);

            // Checking if ModelState is valid and log if content does not meet requirements
            if (!ModelState.IsValid)
            {
                // Log model validation errors
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError("Model validation error for {Key}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }
                return View(order); // Return the view with validation errors
            }

            try
            {
                // Add the order to the database and save changes
                _itemDbContext.Orders.Add(order);           
                await _itemDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving order: {@Order}", order);     //Log if error occurs
                throw; 
            }
            
            // Redirecting to the OrderConfirmation action with the order ID
            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });

        }

        public IActionResult OrderConfirmation(int orderId)
        {
            // Retrieving the order details from the database based on orderId
            var order = _itemDbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);

            // Checking if the order was found
            if (order == null)
            {
                // case where the order with the given ID doesn't exist
                return NotFound();
            }

            // Pass the order to the view
            return View("OrderConfirmation", order);
        }   
    }
}
