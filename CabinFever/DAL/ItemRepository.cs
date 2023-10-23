using Microsoft.EntityFrameworkCore;
using CabinFever.Models;

namespace CabinFever.DAL;

// ItemRepository class that implements the IItemRepository interface
public class ItemRepository : IItemRepository
{
    private readonly ItemDbContext _db; // The database context for working with items

    private readonly ILogger<ItemRepository> _logger; // Logger for logging errors

    // Constructor for the ItemRepository, taking the ItemDbContext and logger as dependencies
    public ItemRepository(ItemDbContext db, ILogger<ItemRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    // Method to retrieve all items asynchronously
    public async Task<IEnumerable<Item>?> GetAll()
    {
        try
        {
            // Return all items by executing a ToListAsync operation on the Items DbSet
            return await _db.Items.ToListAsync();
        }
        catch (Exception e)
        {
            // Log any errors that occur during the operation and return null
            _logger.LogError("[ItemRepository] items ToListAsync() failed when GetAll(), error" +
                "message: {e}", e.Message);
            return null;
        }
    }

    // Method to retrieve an item by its ID asynchronously
    public async Task<Item?> GetItemById(int id)
    {
        try
        {
            // Find an item in the Items DbSet by its ID
            return await _db.Items.FindAsync(id);
        }
        catch (Exception e)
        {
            // Log any errors that occur during the operation and return null
            _logger.LogError("[ItemRepository] item FindAsync(id) failed when GetItemById for " +
                "Id {Id:0000}, error message; {e}", id, e.Message);
            return null;
        }

    }

    // Method to create a new item in the database asynchronously
    public async Task<bool> Create(Item item)
    {
        try
        {
            // Add the item to the Items DbSet
            _db.Items.Add(item);

            // Save the changes to the database
            await _db.SaveChangesAsync();

            // Return true to indicate the success of item creation
            return true;
        }
        catch (Exception e)
        {
            // Log any errors that occur during the operation and return false
            _logger.LogError("[ItemRepository] item creation failed for item {@item}" +
                "error message; {e}", item, e.Message);
            return false;
        }
    }

    // Method to update an item in the database asynchronously
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

    // Method to delete an item by its ID asynchronously
    public async Task<bool> Delete(int id)
    {
        try
        {
            // Find the item by its ID in the database
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                // Log an error if the item is not found and return false
                _logger.LogError("[ItemRepository] item not found for the Id {Id:0000}", id);
                return false;
            }

            // Remove the item from the Items DbSet
            _db.Items.Remove(item);

            // Save the changes to the database
            await _db.SaveChangesAsync();

            // Return true to indicate the success of item deletion
            return true;
        }
        catch (Exception e)
        {
            // Log any errors that occur during the operation and return false
            _logger.LogError("[ItemRepository] item deletion failed for Id {Id:0000}" +
                "error message; {e}", id, e.Message);
            return false;
        }
    }

    // Method to retrieve orders for a specific user
    public async Task<IEnumerable<Order>> GetOrdersForUser(string userId)
    {
        // Query the Orders DbSet to retrieve orders for a specific user
        var orders = await _db.Orders
            .Where(o => o.UserId == userId)
            .ToListAsync();

        return orders;
    }
}