
using Entities.Models.Categories;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.RequestFeatures.ProductsParameters
{
    [OrderByColumns(nameof(Shoes.Size))]
    public class ShoesParameters : ProductsParameters
    {
        [SearchValues("Athletic Shoes", "Casual Shoes", "Dress Shoes", "Boots", "Sandals", "Slippers")]
        [SearchParameter("Type", ExactMatch = true)]
        public string? SearchType { get; set; }
        [SearchValues("Nike", "Adidas", "Puma", "Reebok", "New Balance", "Under Armour", "Converse", "")]
        [SearchParameter("Brand", ExactMatch = true)]
        public string? SearchBrand { get; set; }
        [SearchValues(29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46)]
        [ParameterName("Size")]
        public int? SearchSize { get; set; }
    }
}
