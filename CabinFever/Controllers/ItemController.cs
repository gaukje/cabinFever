using Microsoft.AspNetCore.Authorization;
using CabinFever.DAL;
using Microsoft.AspNetCore.Mvc;
using CabinFever.ViewModels;
using CabinFever.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace CabinFever.Controllers;

public class ItemController : Controller
{
    private readonly IItemRepository _itemRepository;
    private readonly ILogger<ItemController> _logger;
    private readonly UserManager<IdentityUser> _userManager; // Legg til denne linjen
    private readonly IWebHostEnvironment _env;

    public ItemController(IItemRepository itemRepository, ILogger<ItemController> logger, UserManager<IdentityUser> userManager, IWebHostEnvironment env) // Legg til UserManager<IdentityUser> userManager her
    {
        _itemRepository = itemRepository;
        _logger = logger;
        _userManager = userManager; // Og initialiser den her
        _env = env;
    }

    // Private, so the page isn't available for editing on .../Item/Table
    private async Task<IActionResult> Table()
    {
        var items = await _itemRepository.GetAll();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var orders = await _itemRepository.GetOrdersForUser(userId);

        if (items == null)
        {
            _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
            return NotFound("Item list not found");
        }
        var itemListViewModel = new ItemListViewModel(items, "Table", orders);
        return View(itemListViewModel);
    }

    public async Task<IActionResult> Grid()
    {
        var items = await _itemRepository.GetAll();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var orders = await _itemRepository.GetOrdersForUser(userId);

        if (items == null)
        {
            _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
            return NotFound("Item list not found");
        }
        var itemListViewModel = new ItemListViewModel(items, "Grid", orders);
        return View(itemListViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _itemRepository.GetItemById(id);
        if (item == null)
        {
            _logger.LogError("[ItemController] Item not found for the ItemId {ItemId:0000}", id);
            return NotFound("Item not found for the ItemId");
        }

        var user = await _userManager.FindByIdAsync(item.UserId);
        if (user != null)
        {
            var userNameBeforeAt = user.UserName.Split('@')[0]; // Split the username by '@' and take the first part
            ViewData["CreatorName"] = userNameBeforeAt;
        }

        return View(item);
    }



    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Item item, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ViewData["FileError"] = "Please upload an image.";
            return View(item);
        }

        try
        {
            item.UserId = _userManager.GetUserId(User);

            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(_env.WebRootPath, "images", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                item.ImageUrl = "/images/" + file.FileName;

                _logger.LogInformation("ImageUrl is set to: {ImageUrl}", item.ImageUrl);

                // Clear the ModelState error for ImageUrl since we've provided a value now
                ModelState.Remove("ImageUrl");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid for: {@item}", item);

                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        _logger.LogWarning("Validation error on {Field}: {ErrorMessage}", modelStateKey, error.ErrorMessage);
                    }
                }
                return View(item);
            }

            bool returnOk = await _itemRepository.Create(item);
            if (returnOk)
                return RedirectToAction("MinSide", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to create item: {@item}", item);
        }

        return View(item);
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Update(int id)
    {
        var item = await _itemRepository.GetItemById(id);
        if (item == null)
        {
            _logger.LogError("[ItemController] Item not found when updating the ItemId {ItemId:0000}", id);
            return BadRequest("Item not found for the ItemId");
        }
        return View(item);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Update(Item item, IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var filePath = Path.Combine(_env.WebRootPath, "images", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            item.ImageUrl = "/images/" + file.FileName;
            // Clear the ModelState error for ImageUrl since we've provided a value now
            ModelState.Remove("ImageUrl");
        }

        if (ModelState.IsValid)
        {
            bool returnOk = await _itemRepository.Update(item);
            if (returnOk)
                return RedirectToAction("Minside", "Home");
        }
        _logger.LogWarning("[ItemController] Item update failed {@item}", item);
        return View(item);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _itemRepository.GetItemById(id);
        if (item == null)
        {
            _logger.LogError("[ItemController] Item not found for the ItemId {ItemId:0000}", id);
            return BadRequest("Item not found for the ItemId");
        }
        return View(item);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _itemRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[ItemController] Item deletion failed for the ItemId {ItemId:0000}", id);
            return BadRequest("Item deletion failed");
        }
        return RedirectToAction("MinSide", "Home");
    }
}
