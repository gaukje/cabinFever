using CabinFever.Models;

namespace CabinFever.DAL;

// Define the IItemRepository interface
public interface IItemRepository
{
    Task<IEnumerable<Item>?> GetAll();  // Method to get all items as an asynchronous operation
    Task<Item?> GetItemById(int id);   // Method to get an item by ID as an asynchronous operation
    Task<bool> Create(Item item);     // Method to create an item as an asynchronous operation
    Task<bool> Update(Item item);    // Method to update an item as an asynchronous operation
    Task<bool> Delete(int id);      // Method to delete an item by ID as an asynchronous operation
    Task<IEnumerable<Order>> GetOrdersForUser(string userId); // Method to get orders for a user based on their user ID as an asynchronous operation

}
