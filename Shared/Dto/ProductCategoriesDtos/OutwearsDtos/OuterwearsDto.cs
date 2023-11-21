using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;
using Shared.RequestFeatures.ParametersAttributes;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.ProductCategoriesDtos.OuterwearsDtos
{
    
    public record OuterwearsDto : ProductDto
    {
        public string Type { get; set; }
        [PropertyName("Height (cm)")]
        [MinLength(30)]
        [MaxLength(220)]
        public int Height { get; set; }
        public string Brand { get; set; }

    }
}
