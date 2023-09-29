using Microsoft.AspNetCore.Mvc.Rendering;
using CabinFever.Models;

namespace CabinFever.ViewModels
{
    public class CreateOrderItemViewModel
    {
        public OrderItem OrderItem { get; set; } = default!;
        public List<SelectListItem> ItemSelectList { get; set; } = default!;
        public List<SelectListItem> OrderSelectList { get; set; } = default!;
    }
}
