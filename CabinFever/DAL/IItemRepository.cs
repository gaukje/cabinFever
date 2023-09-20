using CabinFever.Models;

namespace CabinFever.DAL;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAll();
    Task<Item?> GetItemById(int id);
    
}
