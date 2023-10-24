using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace CabinFever.Models;

public class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
        UserManager<IdentityUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        // context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Legg til brukere
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

        // Legg til items
        if (!context.Items.Any())
        {
            var user1Id = userManager.FindByEmailAsync("user1@example.com").Result.Id;
            var user2Id = userManager.FindByEmailAsync("user2@example.com").Result.Id;

            var items = new List<Item>
            {
                new Item
                {
                    Name = "Oslomarka",
                    PricePerNight = 2000,
                    Description = "The most outstanding cabin in the heart of Norway",
                    ImageUrl = "/images/hytte_stock_1.jpg",
                    UserId = user1Id,
                    Capacity = 4,
                    Location = "Oslo"
                },

                new Item
                {
                    Name = "Haugesund",
                    PricePerNight = 3000,
                    Description = "Nothing else like the coziest cabin in Haugesund perfect for a romantic get away",
                    ImageUrl = "/images/hytte_stock_2.jpg",
                    UserId = user1Id,
                    Capacity = 2,
                    Location = "Vestland"
                },

                new Item
                {
                    Name = "Geilo",
                    PricePerNight = 4000,
                    Description = "Have an unforgettable night with the family in this memorable viking cabin",
                    ImageUrl = "/images/hytte_stock_3.jpg",
                    UserId = user1Id, 
                    Capacity = 7,
                    Location = "Viken"
                },

                new Item
                {
                    Name = "Jotunheimen",
                    PricePerNight = 2400,
                    Description = "Nelson Mandelas favorite cabin in Norway, RIP",
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
            // Legg til item availabilities
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(60); // 60 dager fra nå

            var itemAvailabilities = new List<ItemAvailability>();

            foreach (var item in context.Items)
            {
                for (var date = startDate; date < endDate; date = date.AddDays(1))
                {
                    var isAvailable = true;

                    // Eksempel: Gjør item utilgjengelig på tilfeldige dager
                    if (date.DayOfWeek == DayOfWeek.Friday && new Random().Next(2) == 0) // 50% sjanse for at en fredag er utilgjengelig
                    {
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

        if (!context.Orders.Any())
        {
            var user1Id = userManager.FindByEmailAsync("user1@example.com").Result.Id;
            var user2Id = userManager.FindByEmailAsync("user2@example.com").Result.Id;
            var orders = new List<Order>
            {
                new Order
                {
                    OrderDate = DateTime.UtcNow,  // Bruker UTC tidspunkt
                    TotalPrice = 4000,
                    ItemId = 1,
                    FromDate = DateTime.UtcNow,  // Bruker UTC tidspunkt
                    ToDate = DateTime.UtcNow.AddDays(5),  // Bruker UTC tidspunkt
                    UserId = user1Id
                },
                new Order
                {
                    OrderDate = DateTime.UtcNow,  // Bruker UTC tidspunkt
                    TotalPrice = 3000,
                    ItemId = 2,
                    FromDate = DateTime.UtcNow,  // Bruker UTC tidspunkt
                    ToDate = DateTime.UtcNow.AddDays(3),  // Bruker UTC tidspunkt
                    UserId = user2Id
                },
            };
            context.AddRange(orders);
            context.SaveChanges();
        }
    }
}

