using Microsoft.AspNetCore.Mvc.Rendering;
using CabinFever.Models;

namespace CabinFever.ViewModels
{
    public class CreateOrderViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MinCheckInDate { get; set; }
        public string MaxCheckOutDate { get; set; }
        public int MaxGuests { get; set; }
    }
}
