namespace CabinFever.Models;

public class Order
{
    public int OrderId { get; set; }
    public string OrderDate { get; set; } = string.Empty;
    public int UserId { get; set; }
    // navigation property
    // public virtual User User { set; get; } = default!;
    // navigation property
    public virtual List<OrderItem>? OrderItems { get; set; }
    public decimal TotalPrice { get; set; }
}

