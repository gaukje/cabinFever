using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinFever.Models;
public class ItemAvailability
{
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public bool IsAvailable { get; set; }

    // Foreign key for Item table.
    [Required]
    public int ItemId { get; set; }

    // Navigation property
    [ForeignKey("ItemId")]
    public virtual Item? Item { get; set; }
}
