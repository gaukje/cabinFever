using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace CabinFever.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; } = string.Empty;

        // Foreign key for AspNetUsers table.
        public string UserId { get; set; }

        // Navigation property
        public virtual IdentityUser User { get; set; } = default!;

        public decimal TotalPrice { get; set; }

        // Foreign key for Item table.
        public int ItemId { get; set; }

        // Navigation property
        public virtual Item Item { get; set; } = default!;

        [Required(ErrorMessage = "Please select a From Date")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Please select a To Date")]
        public DateTime ToDate { get; set; }
        /*
        [Required(ErrorMessage = "Please specify the number of guests")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of guests must be at least 1")]
        */
        public int Guests { get; set; }
    }
}