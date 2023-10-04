using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

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
        public virtual List<OrderItem>? OrderItems{ get; set; }
    }
}
