using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.ProductsParameters;

namespace Contracts.RepositoryContracts
{
    public interface IProductRepository
    {
        Task<PagedList<ProductDto>> GetAllProductByCategory(string category, ProductsParameters parameters);
        Task<(ProductDto, Dictionary<string, string>)> GetProductById(string category, Guid productId);
        Task<Guid> CreateProduct(string category, ProductForManipulating product);
        Task<(PagedList<ProductDto> pagedResult, long summaryPrice)> GetProductsByCart(string userId, ProductsParameters productParameters);
        Task UpdateProduct(string category, ProductForManipulating product, Guid id);
        Task DeleteProduct(string category, Guid productId);
    }
}
