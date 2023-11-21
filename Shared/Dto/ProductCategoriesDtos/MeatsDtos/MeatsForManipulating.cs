using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.ProductCategoriesDtos.MeatsDtos
{
    public record MeatsForManipulating : ProductForManipulating
    {
        [Required(ErrorMessage = "MeatType is required")]
        [DefaultValues(true, "Beef", "Pork", "Chicken", "Lamb", "Turkey")]
        public string MeatType { get; set; }
        [Required(ErrorMessage = "DateCreated is required")]
        public DateOnly? DateCreated { get; set; }
    }
}
