using Microsoft.AspNetCore.Mvc.Rendering;
using CabinFever.Models;

namespace CabinFever.ViewModels
{
    public class CreateOrderViewModel
    {
        public int ItemId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Guests { get; set; }
    }
}
