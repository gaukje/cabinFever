using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CabinFever.Models
{
    public class ItemDbContext : IdentityDbContext
    {
        public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
        {
            // Database.EnsureCreated();
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ItemAvailability> ItemAvailability { get; set; } // Add this line

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Important to call this when extending IdentityDbContext

            // Configure relationship between Order and Item
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Item)
                .WithMany(i => i.Orders)
                .HasForeignKey(o => o.ItemId)
                .OnDelete(DeleteBehavior.SetNull);

            // Optionally, add more configurations for ItemAvailability here if needed
        }
    }
}
