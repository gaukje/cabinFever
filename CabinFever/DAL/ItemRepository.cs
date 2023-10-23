using Microsoft.EntityFrameworkCore;
using CabinFever.Models;

namespace CabinFever.DAL;

public class ItemRepository : IItemRepository
{
    private readonly ItemDbContext _db;

    private readonly ILogger<ItemRepository> _logger;

    public ItemRepository(ItemDbContext db, ILogger<ItemRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Item>?> GetAll()
    {
        try
        {
            return await _db.Items.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] items ToListAsync() failed when GetAll(), error" +
                "message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Item?> GetItemById(int id)
    {
        try
        {
            return await _db.Items.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] item FindAsync(id) failed when GetItemById for " +
                "Id {Id:0000}, error message; {e}", id, e.Message);
            return null;
        }

    }

    public async Task<bool> Create(Item item)
    {
        try
        {
            _db.Items.Add(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] item creation failed for item {@item}" +
                "error message; {e}", item, e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Item item)
    {
        try
        {
            // Find the existing item in the database using the item's Id
            var existingItem = await _db.Items.FindAsync(item.Id);

            // If an existing item is found, detach it from the DbContext
            // This is to avoid Entity Framework from tracking two instances with the same key
            if (existingItem != null)
            {
                _db.Entry(existingItem).State = EntityState.Detached;
            }

            // Update the item in the DbSet
            // This marks the entity as Modified, so that it will be updated in the database when SaveChanges is called
            _db.Items.Update(item);

            // Save the changes to the database
            await _db.SaveChangesAsync();

            // Return true to indicate that the update was successful
            return true;
        }
        catch (Exception e)
        {
            // Log any errors that occur during the update
            _logger.LogError("[ItemRepository] item FindAsync(id) failed when updating the Id" +
                "{Id:0000}, error message; {e}", item, e.Message);

            // Return false to indicate that the update failed
            return false;
        }
    }


    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                _logger.LogError("[ItemRepository] item not found for the Id {Id:0000}", id);
                return false;
            }

            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] item deletion failed for Id {Id:0000}" +
                "error message; {e}", id, e.Message);
            return false;
        }
    }

    public async Task<IEnumerable<Order>> GetOrdersForUser(string userId)
    {
        var orders = await _db.Orders
            .Where(o => o.UserId == userId)
            .ToListAsync();

        return orders;
    }
}