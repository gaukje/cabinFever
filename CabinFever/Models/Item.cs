namespace CabinFever.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public string? Fylke { get; set; }
        public string? Location { get; set; }
        public string? ImageUrl { get; set; }
        public Boolean? IsAvailable { get; set; }
    }
}
