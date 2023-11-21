using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.Dto.ProductCategoriesDtos.ToysDtos
{
    public record ToysForManipulating : ProductForManipulating
    {
        [DefaultValues(true, "Plush Toys", "Dolls", "Building Toys", "Action Figures", "Educational Toys", "Puzzles")]
        public string Type { get; set; }

    }
}
