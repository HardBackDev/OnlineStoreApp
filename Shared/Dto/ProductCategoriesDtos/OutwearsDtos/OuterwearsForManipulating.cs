using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;

namespace Shared.Dto.ProductCategoriesDtos.OuterwearsDtos
{
    public record OuterwearsForManipulating : ProductForManipulating
    {
        [DefaultValues(true, "Jackets", "Coats", "Vests", "Rainwear", "T-Shirts", "Shirts")]
        public string Type { get; set; }
        public int Height { get; set; }
        [DefaultValues(true, "The North Face", "Patagonia", "Columbia", "Arc'teryx", "Canada Goose", "Helly Hansen", "Moncler")]
        public string Brand { get; set; }
    }
}
