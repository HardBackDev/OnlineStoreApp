
using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.ProductsParameters;

namespace Contracts.ServiceContracts
{
    public interface IProductService
    {
        Task<PagedList<ProductDto>> GetProductsByCategory(string category, ProductsParameters parameters);
        Task<(ProductDto, Dictionary<string, string>)> GetProductById(string category, Guid productId);
        Task<ProductDto> CreateProduct(string category, ProductForManipulating product);
        Task UpdateProduct(string category, ProductForManipulating product, Guid id);
        Task DeleteProduct(string category, Guid productId);

    }
}
