using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.ProductDtos
{
    public record ProductForManipulating
    {
        [Required(ErrorMessage = "Price is required")]
        public int? Price { get; set; }
        [MaxLength(400, ErrorMessage = "Maximum length for the Title is 40 characters")]
        [Required(ErrorMessage = "Price is required")]
        public string Title { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
