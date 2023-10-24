using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace CabinFever.Models
{
    // This class represents an order made by a user for a specific item (e.g., cabin).
    public class Order
    {
        public int OrderId { get; set; }

        // The date when the order was made, initialized to the current UTC time.
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // Foreign key for AspNetUsers table.
        public string? UserId { get; set; }

        // Navigation property
        public virtual IdentityUser? User { get; set; } = default!;

        // The total price of the order.
        public decimal TotalPrice { get; set; }

        // Foreign key for Item table.
        public int ItemId { get; set; }

        // Navigation property
        public virtual Item? Item { get; set; } = default!;

        // The starting date for the order.
        [Required(ErrorMessage = "Please select a From Date")]
        public DateTime FromDate { get; set; }

        // The ending date for the order.
        [Required(ErrorMessage = "Please select a To Date")]
        public DateTime ToDate { get; set; }

        // The number of guests associated with the order.
        [Required(ErrorMessage = "Please specify the number of guests")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of guests must be at least 1")]
        public int Guests { get; set; }
    }
}
