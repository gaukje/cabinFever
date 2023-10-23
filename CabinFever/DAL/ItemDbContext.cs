using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CabinFever.Models
{
    // ItemDbContext class, which extends IdentityDbContext
    public class ItemDbContext : IdentityDbContext
    {
        // Constructor that takes options for configuring the database context
        public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
        {
            // Initialize the database context. Commented out, you can use it to create the database if it doesn't exist.
            // Database.EnsureCreated();
        }

        // DbSet for Items, representing a table for storing Item objects
        public DbSet<Item> Items { get; set; }

        // DbSet for Orders, representing a table for storing Order objects
        public DbSet<Order> Orders { get; set; }

        // DbSet for ItemAvailability, representing a table for storing ItemAvailability objects
        public DbSet<ItemAvailability> ItemAvailability { get; set; } // Add this line

        // Override the OnConfiguring method to specify options for the context
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Enable lazy loading proxies for related entities
            optionsBuilder.UseLazyLoadingProxies();
        }

        // Override the OnModelCreating method to define the database model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base implementation to ensure IdentityDbContext configurations are applied
            base.OnModelCreating(modelBuilder); // Important to call this when extending IdentityDbContext

            // Configure relationship between Order and Item
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Item) // An Order has one associated Item
                .WithMany(i => i.Orders) // An Item can have many associated Orders
                .HasForeignKey(o => o.ItemId) // Define the foreign key for the relationship
                .OnDelete(DeleteBehavior.SetNull); // Specify the deletion behavior

            // Optionally, add more configurations for ItemAvailability here if needed
        }
    }
}
