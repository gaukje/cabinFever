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
        private readonly ItemDbContext _itemDbContext;  // Define a reference to the ItemDbContext
        private readonly ILogger<OrderController> _logger; // Define a logger for OrderController

        public OrderController(ItemDbContext itemDbContext, ILogger<OrderController> logger)
        {
            _itemDbContext = itemDbContext; // Initialize the ItemDbContext
            _logger = logger; // Initialize the logger
        }

        // An action method to display a table of orders
        public async Task<IActionResult> Table()
        {
            // Retrieve a list of orders from the database
            List<Order> orders = await _itemDbContext.Orders.ToListAsync();

            // Return a view with the list of orders
            return View(orders);
        }

        // Define an action method to create a new order (GET request)
        [HttpGet]
        public IActionResult Create()
        {
            // Return a view for creating a new order
            return View();
        }

        // Define an action method to retrieve date ranges for a specific item (GET request)
        [HttpGet]
        public IActionResult GetDateRange(int itemId)
        {
            // Retrieve date ranges from the database based on the item ID
            var dateRanges = _itemDbContext.Orders.Where(order => order.ItemId == itemId && order.ToDate >= DateTime.Today)
                .Select(order => new { order.FromDate, order.ToDate })
                .ToList();

            // Create a list of date strings from the retrieved date ranges
            var dateList = new List<String>();

            foreach (var dateRange in dateRanges)
            {
                for (var date = dateRange.FromDate; date <= dateRange.ToDate; date = date.AddDays(1))
                {
                    var stringDate = date.ToString("yyyy-MM-dd");
                    dateList.Add(stringDate);
                }
            }

            // Return the dateList as JSON
            return Json(dateList);
        }

        // Define an action method to create a new order (POST request)
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            // Retrieve the user's ID from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);        

            // Set the user's ID on the order
            order.UserId = userId;                                             

            // Log the order state
            _logger.LogInformation("Order before saving: {@Order}", order);

            // Manually update the ModelState
            ModelState.Clear();
            TryValidateModel(order);

            // Check if ModelState is valid and 'logger' if user has given info that does not fulfill the requirments
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
                _logger.LogError(ex, "Error occurred while saving order: {@Order}", order);     //Log hvis feil oppstår
                throw; 
            }
            
            // Redirect to the OrderConfirmation action with the order ID
            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });

        }

        public IActionResult OrderConfirmation(int orderId)
        {
            // Retrieve the order details from the database based on orderId
            var order = _itemDbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);

            // Check if the order was found
            if (order == null)
            {
                // Handle the case where the order with the given ID doesn't exist
                return NotFound(); // You might want to return a 404 Not Found response or handle it differently
            }

            // Pass the order to the view
            return View("OrderConfirmation", order); // Here you're passing the order to the view
        }   
    }
}
