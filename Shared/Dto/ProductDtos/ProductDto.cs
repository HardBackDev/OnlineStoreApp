
namespace Shared.Dtos.ProductDtos
{
    public record ProductDto
    {
        public Guid Id { get; init; }
        public int? Price { get; init; }
        public string Title { get; init; }
        public string Category { get; set; }
        public string? PhotoUrl { get; init; }
    }
}
