using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace CabinFever.Models;

public class DBInit
{
    // This method is responsible for seeding initial data into the database.
    public static void Seed(IApplicationBuilder app)
    {
        // Create a scope to access services.
        using var serviceScope = app.ApplicationServices.CreateScope();
        ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
        UserManager<IdentityUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        // context.Database.EnsureDeleted();

        // Ensure the database is created.
        context.Database.EnsureCreated();

        // Add users if they don't exist.
        if (!context.Users.Any())
        {
            var users = new List<IdentityUser>
        {
            new IdentityUser { UserName = "user1@example.com", Email = "user1@example.com" },
            new IdentityUser { UserName = "user2@example.com", Email = "user2@example.com" }
        };

            foreach (var user in users)
            {
                userManager.CreateAsync(user, "passORD1!").Wait();
            }
        }

        // Add items if they don't exist.
        if (!context.Items.Any())
        {
            var user1Id = userManager.FindByEmailAsync("user1@example.com").Result.Id;
            var user2Id = userManager.FindByEmailAsync("user2@example.com").Result.Id;

            var items = new List<Item>
            {
                new Item
                {
                    Name = "Hytte1",
                    PricePerNight = 2000,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_1.jpg",
                    UserId = user1Id,
                    Capacity = 4,
                    Location = "Oslo"
                },

                new Item
                {
                    Name = "Hytte2",
                    PricePerNight = 3000,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_2.jpg",
                    UserId = user1Id,
                    Capacity = 2,
                    Location = "Vestland"
                },

                new Item
                {
                    Name = "Hytte3",
                    PricePerNight = 4000,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_3.jpg",
                    UserId = user1Id, 
                    Capacity = 7,
                    Location = "Viken"
                },

                new Item
                {
                    Name = "Hytte4",
                    PricePerNight = 2400,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_4.jpg",
                    UserId = user2Id,
                    Capacity = 5,
                    Location = "Innlandet"
                },
            };
            context.AddRange(items);
            context.SaveChanges();
        }

        if (!context.ItemAvailability.Any())
        {
            // Add item availabilities if they don't exist.
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(60); // 60 days from now

            var itemAvailabilities = new List<ItemAvailability>();

            foreach (var item in context.Items)
            {
                for (var date = startDate; date < endDate; date = date.AddDays(1))
                {
                    var isAvailable = true;

                    // Example: Make items unavailable on random days.
                    if (date.DayOfWeek == DayOfWeek.Friday && new Random().Next(2) == 0) // 50% sjanse for at en fredag er utilgjengelig
                    {
                        // 50% chance for an item to be unavailable on a Friday.
                        isAvailable = false;
                    }

                    itemAvailabilities.Add(new ItemAvailability
                    {
                        Date = date,
                        IsAvailable = isAvailable,
                        ItemId = item.Id
                    });
                }
            }

            context.AddRange(itemAvailabilities);
            context.SaveChanges();
        }

        // Add orders if they don't exist.
        if (!context.Orders.Any())
        {
            var user1Id = userManager.FindByEmailAsync("user1@example.com").Result.Id;
            var user2Id = userManager.FindByEmailAsync("user2@example.com").Result.Id;
            var orders = new List<Order>
            {
                new Order
                {
                    OrderDate = DateTime.UtcNow,  // Use UTC timestamp
                    TotalPrice = 4000,
                    ItemId = 1,
                    FromDate = DateTime.UtcNow,  // Use UTC timestamp
                    ToDate = DateTime.UtcNow.AddDays(5),  // Use UTC timestamp
                    UserId = user1Id
                },
                new Order
                {
                    OrderDate = DateTime.UtcNow,  // Use UTC timestamp
                    TotalPrice = 3000,
                    ItemId = 2,
                    FromDate = DateTime.UtcNow,  // Use UTC timestamp
                    ToDate = DateTime.UtcNow.AddDays(3),  // Use UTC timestamp
                    UserId = user2Id
                },
            };
            context.AddRange(orders);
            context.SaveChanges();
        }
    }
}

