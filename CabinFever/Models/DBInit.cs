using Microsoft.EntityFrameworkCore;
using System;

namespace CabinFever.Models;

public class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
       // context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Items.Any())
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Hytte1",
                    PricePerNight = 2000,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_1.jpg"
                },

                new Item
                {
                    Name = "Hytte2",
                    PricePerNight = 3000,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_2.jpg"
                },

                new Item
                {
                    Name = "Hytte3",
                    PricePerNight = 4000,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_3.jpg"
                },

                new Item
                {
                    Name = "Hytte4",
                    PricePerNight = 2400,
                    Description = "Ekstrem hytte",
                    ImageUrl = "/images/hytte_stock_4.jpg"
                },
            };
            context.AddRange(items);
            context.SaveChanges();
        }

        /*
        if (!context.Orders.Any())
        {
            var orders = new List<Order>
            {
                new Order { OrderDate = DateTime.Now.ToString("yyyy-MM-dd"), TotalPrice = 4000 },
                new Order { OrderDate = DateTime.Now.ToString("yyyy-MM-dd"), TotalPrice = 3000 },

            };
            context.AddRange(orders);
            context.SaveChanges();

            // Tilknytt Items til Orders
            var firstOrder = orders[0];
            var secondOrder = orders[1];

            var firstItem = context.Items.Find(1);
            var secondItem = context.Items.Find(2);
            var thirdItem = context.Items.Find(3);

            firstOrder.Items.Add(firstItem);
            firstOrder.Items.Add(secondItem);
            secondOrder.Items.Add(thirdItem);

            context.SaveChanges();
        }
        */
    }
}