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
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _itemRepository.GetItemById(id);
        if (item == null)
            return BadRequest("item not found");
        return View(item);
    }


}


