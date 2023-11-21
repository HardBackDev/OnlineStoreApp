using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures;

namespace Shared.Dto.CartDtos
{
    public record CartDto
    {
        public string UserId { get; init; }
        public long? SummaryPrice { get; init; }
        public PagedList<ProductDto> products { get; init; }
    };
}
