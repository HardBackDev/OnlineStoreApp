using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.Dto.ProductCategoriesDtos.ToysDtos
{
    public record ToysDto : ProductDto
    {
        public string Type { get; set; }

    }
}
