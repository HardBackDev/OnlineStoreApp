using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.DtoAttributes;

namespace Shared.Dto.ProductCategoriesDtos.MeatsDtos
{
    public record MeatsDto : ProductDto
    {
        [PropertyName("Meat Type")]
        public string MeatType { get; set; }
        [PropertyName("Date Created")]
        public DateOnly DateCreated { get; set; }
    }
}
