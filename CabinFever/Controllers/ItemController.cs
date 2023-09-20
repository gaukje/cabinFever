using CabinFever.DAL;
using Microsoft.AspNetCore.Mvc;
using CabinFever.ViewModels;
using CabinFever.Models;


namespace CabinFever.Controllers;

public class ItemController : Controller
{
    private readonly IItemRepository _itemRepository;
    private readonly ILogger<ItemController> _logger;

    public ItemController(IItemRepository itemRepository, ILogger<ItemController>
        logger)
    {
        _itemRepository = itemRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Grid()
    {
        _logger.LogInformation("This is an information message.");
        _logger.LogWarning("This is a warning message.");
        _logger.LogError("This is an error message.");

        var items = await _itemRepository.GetAll();
        var itemListViewModel = new ItemListViewModel(items, "Grid");
        return View(itemListViewModel);

        //List<Item> items = await _itemDbContext.Items.ToListAsync();
        //var itemListViewModel = new ItemListViewModel(items, "Grid");
        //return View(itemListViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _itemRepository.GetItemById(id);
        if (item == null)
            return BadRequest("item not found");
        return View(item);

        //var item = await _itemDbContext.Items.FindAsync(id);
        //if (item == null)
        //{
        //    return NotFound();
        //}
        //return View(item);
    }
}


//public List<Item> GetItems()
//{
//    var items = new List<Item>();
//    var item1 = new Item
//    {
//        Id = 1,
//        Name = "Fjellslott",
//        PricePerNight = 2100,
//        Capacity = 4,
//        Description = "An amazing cabin at Fjellslott",
//        ImageUrl = "/images/hytte_stock_1.jpg"
//    };

//    var item2 = new Item
//    {
//        Id = 2,
//        Name = "Kragerø",
//        PricePerNight = 3500,
//        Capacity = 5,
//        Description = "This cabin is gonna blow you away",
//        ImageUrl = "/images/hytte_stock_2.jpg"
//    };

//    var item3 = new Item
//    {
//        Id = 3,
//        Name = "Lofoten",
//        PricePerNight = 4300,
//        Capacity = 6, 
//        Description = "Once in a lifetime cabin experience in Lofoten",
//        ImageUrl = "/images/hytte_stock_3.jpg"
//    };

//    items.Add(item1);
//    items.Add(item2);
//    items.Add(item3);
//    return items;
//}
