using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;
using Shared.RequestFeatures.ParametersAttributes;

namespace Shared.Dto.ProductCategoriesDtos.ShoesDtos
{
    public record ShoesDto : ProductDto
    {
        public string Type { get; set; }
        public string Brand { get; set; }
        public int Size { get; set; }
    }
}
