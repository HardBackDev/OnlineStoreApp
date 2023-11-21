using Entities.Models.Categories;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.RequestFeatures.ProductsParameters
{
    [OrderByColumns(nameof(Outerwears.Height))]
    public class OuterwearsParameters : ProductsParameters
    {
        [SearchValues("Jackets", "Coats", "Vests", "Rainwear", "T-Shirts", "Shirts")]
        [SearchParameter("Type", ExactMatch = true)]
        public string? SearchType { get; set; }

        [SearchValues("The North Face", "Patagonia", "Columbia", "Arc'teryx", "Canada Goose", "Helly Hansen", "Moncler")]
        [SearchParameter("Brand", ExactMatch = true)]
        public string? SearchBrand { get; set; }

        [ParameterName("Min Height (cm)")]
        public int MinHeight { get; set; } = 0;
        [ParameterName("Max Height (cm)")]
        public int MaxHeight { get; set; } = int.MaxValue;
    }
}
