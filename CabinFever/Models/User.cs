using Microsoft.AspNetCore.Mvc;

namespace CabinFever.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Navn{ get; set; } = string.Empty;
        public string Brukernavn { get; set; }
        public string Epost { get; set; }
        public string Passord { get; set; }
        // navigation property
        public virtual List<Order>? Orders { get; set; }
    }
}
