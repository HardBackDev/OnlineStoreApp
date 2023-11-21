
namespace Entities.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
