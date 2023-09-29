using Microsoft.AspNetCore.Mvc;

namespace CabinFever.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name{ get; set; } = string.Empty;
        public string Username{ get; set; }
        public string Mail{ get; set; }
        public string Password{ get; set; }
        // navigation property
        public virtual List<Order>? Orders { get; set; }
    }
}
