using Microsoft.EntityFrameworkCore;
using CabinFever.Models;

namespace CabinFever.DAL;

public class ItemRepository : IItemRepository
{
    private readonly ItemDbContext _db;

    public ItemRepository(ItemDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Item>> GetAll()
    {
        return await _db.Items.ToListAsync();
    }

    public async Task<Item?> GetItemById(int id)
    {
        return await _db.Items.FindAsync(id);
    }
}

