using Microsoft.EntityFrameworkCore;

namespace CabinFever.Models;

public class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
        context.Database.EnsureDeleted();
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
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User { Navn = "Alice Hansen"},
                new User { Navn = "Bob Johansen"},
            };
            context.AddRange(users);
            context.SaveChanges();
        }

        if (!context.Orders.Any())
        {
            var orderItems = new List<OrderItem>
            {
                new OrderItem { ItemId = 1, AmountNights = 2, OrderId = 1},
                new OrderItem { ItemId = 2, AmountNights = 1, OrderId = 1},
                new OrderItem { ItemId = 3, AmountNights = 3, OrderId = 2},
            };
            foreach (var orderItem in orderItems)
            {
                var item = context.Items.Find(orderItem.ItemId);
                orderItem.OrderItemPrice = orderItem.Price;
            }
        }
    }
}
