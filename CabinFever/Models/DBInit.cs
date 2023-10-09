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
                    Name = "Hytte1",
                    PricePerNight = 2000,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_1.jpg",
                    UserId = user1Id
                },

                new Item
                {
                    Name = "Hytte2",
                    PricePerNight = 3000,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_2.jpg",
                    UserId = user1Id
                },

                new Item
                {
                    Name = "Hytte3",
                    PricePerNight = 4000,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_3.jpg",
                    UserId = user1Id
                },

                new Item
                {
                    Name = "Hytte4",
                    PricePerNight = 2400,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_4.jpg",
                    UserId = user2Id
                },
            };
            context.AddRange(items);
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
                    OrderDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    TotalPrice = 4000,
                    ItemId = 1,
                    FromDate = DateTime.Now,  // Legg til en verdi for FromDate
                    ToDate = DateTime.Now.AddDays(5),  // Legg til en verdi for ToDate
                    UserId = user1Id
                },
                new Order
                {
                    OrderDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    TotalPrice = 3000,
                    ItemId = 2,
                    FromDate = DateTime.Now,  // Legg til en verdi for FromDate
                    ToDate = DateTime.Now.AddDays(3),  // Legg til en verdi for ToDate
                    UserId = user2Id
                },
            };
            context.AddRange(orders);
            context.SaveChanges();
        }

    }
}

