using Microsoft.AspNetCore.Mvc.Rendering;
using CabinFever.Models;

namespace CabinFever.ViewModels
{
    public class CreateOrderItemViewModel
    {
        public Order Order { get; set; } = new Order();
        public List<SelectListItem> ItemSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> UserSelectList { get; set; } = new List<SelectListItem>(); // Hvis du vil la brukeren velge en bruker fra en liste
    }
}
