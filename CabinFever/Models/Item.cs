using System.ComponentModel.DataAnnotations;

namespace CabinFever.Models
    
{
    public class Item
    {
        public int Id { get; set; }

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The Name must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Item name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price must be greater than 0.")]
        public decimal PricePerNight { get; set; }

        public int Capacity { get; set; }

        [StringLength(200)]
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
