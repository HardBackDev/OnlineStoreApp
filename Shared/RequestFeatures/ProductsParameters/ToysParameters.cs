using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.RequestFeatures.ProductsParameters
{
    public class ToysParameters : ProductsParameters
    {
        [SearchValues("Plush Toys", "Dolls", "Building Toys", "Action Figures", "Educational Toys", "Puzzles")]
        [SearchParameter("Type", ExactMatch = true)]
        public string? SearchType { get; set; }
    }
}
