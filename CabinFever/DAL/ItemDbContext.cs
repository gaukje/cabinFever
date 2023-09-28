using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CabinFever.Models;

public class ItemDbContext : IdentityDbContext
{
    public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
    {
       // Database.EnsureCreated();
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}

