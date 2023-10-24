using Microsoft.AspNetCore.Mvc.Rendering;
using CabinFever.Models;

namespace CabinFever.ViewModels
{
    // This view model class is used to represent the data required for creating a new order.
    public class CreateOrderViewModel
    {
        // The ID of the item (cabin) for which the order is being created.
        public int ItemId { get; set; }

        // The starting date for the order.
        public DateTime FromDate { get; set; }

        // The ending date for the order.
        public DateTime ToDate { get; set; }

        // The number of guests associated with the order.
        public int Guests { get; set; }
    }
}
