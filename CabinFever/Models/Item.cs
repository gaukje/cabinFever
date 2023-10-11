using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CabinFever.Models
{
    public class Item
    {
        public int Id { get; set; }

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The Name must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Cabin name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price must be greater than 0.")]
        public decimal PricePerNight { get; set; }

        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please select a From Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }

        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please select a To Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }

        public int Capacity { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public string? Fylke { get; set; }

        public string? Location { get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        public bool? IsAvailable { get; set; }

        // Foreign key for AspNetUsers table.
        public string? UserId { get; set; }

        // Navigation property
        public virtual IdentityUser? User { get; set; }

        // Navigation property for Orders
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Navigation property for ItemAvailability
        public virtual ICollection<ItemAvailability> ItemAvailabilities { get; set; } = new List<ItemAvailability>();
    }
}
