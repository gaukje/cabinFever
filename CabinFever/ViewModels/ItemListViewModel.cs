using System;
using System.Collections.Generic;
using CabinFever.Models;

namespace CabinFever.ViewModels
{
	public class ItemListViewModel
	{
		public IEnumerable<Item> Items;
		public string? CurrentViewName;
		public IEnumerable<Order> Orders {  get; set; }

		public ItemListViewModel(IEnumerable<Item> items, string? currentViewName, IEnumerable<Order> orders)
        {
            Items = items;
            CurrentViewName = currentViewName;
            Orders = orders;
        }
    }
}

