using Microsoft.AspNetCore.Mvc;

namespace CabinFever.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name{ get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Passord { get; set; } = string.Empty;
        // navigation property
        public virtual List<Order>? Orders { get; set; }
    }
}
