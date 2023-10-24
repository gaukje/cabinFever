using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CabinFever.Models
{
    // This class represents an item in the application, typically a cabin or accommodation.
    public class Item
    {
        public int Id { get; set; }

        // The name of the cabin, must consist of 2 to 20 alphanumeric characters, and can contain spaces, periods, hyphens, and special characters like 'æøåÆØÅ'.
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The Name must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Cabin name")]
        public string Name { get; set; } = string.Empty;

        // The price per night for renting the cabin, must be greater than 0. A must
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price must be greater than 0.")]
        public decimal PricePerNight { get; set; }

        // The starting date for cabin availability. A must
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please select a From Date.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }

        // The ending date for cabin availability. A must
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please select a To Date.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }

        // The maximum capacity of the cabin, must be greater than 0. A must
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The Capacity must be greater than 0.")]
        public int Capacity { get; set; }

        // A description of the cabin, with a maximum length of 5000 characters. A must
        [Required]
        [StringLength(5000)]
        public string? Description { get; set; }

        public string? Fylke { get; set; }

        // The location of the cabin (e.g., city or region). A must
        [Required]
        public string? Location { get; set; }

        // The URL of an image representing the cabin. A must
        [Required]
        public string? ImageUrl { get; set; }

        // Indicates whether the cabin is currently available.
        public bool? IsAvailable { get; set; }

        // Foreign key for the associated user (owner) in the AspNetUsers table.
        public string? UserId { get; set; }

        // Navigation property to access the associated user.
        public virtual IdentityUser? User { get; set; }

        // Navigation property to access the orders associated with this item.
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Navigation property to access the availability schedule for this item.
        public virtual ICollection<ItemAvailability> ItemAvailabilities { get; set; } = new List<ItemAvailability>();
    }
}
