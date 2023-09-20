using Microsoft.AspNetCore.Mvc;

namespace CabinFever.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string Brukernavn { get; set; }
        public string Epost { get; set; }
        public string Passord { get; set; }
        // navigation property
        public virtual List<Order>? Orders { get; set; }
    }
}
