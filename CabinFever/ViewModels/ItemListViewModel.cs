using System;
using System.Collections.Generic;
using CabinFever.Models;

namespace CabinFever.ViewModels
{
    // This view model class is used to represent a list of items, a view name, and a list of orders.
    public class ItemListViewModel
	{
        // A collection of items to be displayed.
        public IEnumerable<Item> Items;

        // The name of the current view.
        public string? CurrentViewName;

        // A collection of orders related to the items.
        public IEnumerable<Order> Orders {  get; set; }

        // Constructor to initialize the view model with items, a view name, and orders.
        public ItemListViewModel(IEnumerable<Item> items, string? currentViewName, IEnumerable<Order> orders)
        {
            Items = items;
            CurrentViewName = currentViewName;
            Orders = orders;
        }
    }
}

