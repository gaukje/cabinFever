using System.ComponentModel.DataAnnotations;

namespace CabinFever.Models
    
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public string? Fylke { get; set; }
        public string? Location { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
        public Boolean? IsAvailable { get; set; }
        // navigation property
        public virtual List<OrderItem>? OrderItems { get; set; }
    }
}
