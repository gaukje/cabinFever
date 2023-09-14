using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CabinFever.Models;
using Microsoft.AspNetCore.Mvc;
using CabinFever.ViewModels;


namespace CabinFever.Controllers;

public class ItemController : Controller
{
    //public IActionResult Table()
    //{
        //var items = GetItems();
        //ViewBag.CurrentViewName = "Table";
        //return View(items);
    //}

    public IActionResult Grid()
    {
        var items = GetItems();
        var itemListViewModel = new ItemListViewModel(items, "Grid");
        return View(itemListViewModel);
    }

    public List<Item> GetItems()
    {
        var items = new List<Item>();
        var item1 = new Item
        {
            Id = 1,
            Name = "Fjellslott",
            PricePerNight = 2100,
            Capacity = 4,
            Description = "An amazing cabin at Fjellslott",
            ImageUrl = "/images/hytte_stock_1.jpg"
        };

        var item2 = new Item
        {
            Id = 2,
            Name = "Kragerø",
            PricePerNight = 3500,
            Capacity = 5,
            Description = "This cabin is gonna blow you away",
            ImageUrl = "/images/hytte_stock_2.jpg"
        };

        var item3 = new Item
        {
            Id = 3,
            Name = "Lofoten",
            PricePerNight = 4300,
            Capacity = 6, 
            Description = "Once in a lifetime cabin experience in Lofoten",
            ImageUrl = "/images/hytte_stock_3.jpg"
        };

        items.Add(item1);
        items.Add(item2);
        items.Add(item3);
        return items;
    }
}
