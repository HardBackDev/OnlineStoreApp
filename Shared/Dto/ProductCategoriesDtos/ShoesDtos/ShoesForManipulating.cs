using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;

namespace Shared.Dto.ProductCategoriesDtos.ShoesDtos
{
    public record ShoesForManipulating : ProductForManipulating
    {
        [DefaultValues(true, "Athletic Shoes", "Casual Shoes", "Dress Shoes", "Boots", "Sandals", "Slippers")]
        public string Type { get; set; }
        [DefaultValues(true, "Nike", "Adidas", "Puma", "Reebok", "New Balance", "Under Armour", "Converse")]
        public string Brand { get; set; }
        [DefaultValues(true, 29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46)]
        public int Size { get; set; }

    }
}
