using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinFever.Models;

// This class represents the availability of an item (e.g., cabin) for a specific date.
public class ItemAvailability
{
    public int Id { get; set; }

    // The date for which the item's availability is being defined.
    [Required]
    public DateTime Date { get; set; }

    // Indicates whether the item is available for the specified date.
    [Required]
    public bool IsAvailable { get; set; }

    // Foreign key for the associated item (cabin) in the Item table.
    [Required]
    public int ItemId { get; set; }

    // Navigation property to access the associated item.
    [ForeignKey("ItemId")]
    public virtual Item? Item { get; set; }
}
