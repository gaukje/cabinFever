using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CabinFever.Models;

namespace CabinFever.Models;

public class ItemDbContext : IdentityDbContext
{
    public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
    {
       // Database.EnsureCreated();
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Important to call this when extending IdentityDbContext

        // Configure relationship between Order and Item
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Item)  // Assuming you add a navigation property of type Item in Order class
            .WithMany()
            .HasForeignKey(o => o.ItemId)  // Assuming you add a foreign key property named ItemId in Order class
            .OnDelete(DeleteBehavior.SetNull); // Adjust as needed, e.g. .OnDelete(DeleteBehavior.Cascade);
    }

}

