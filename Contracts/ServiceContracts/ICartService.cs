using Shared.Dto.CartDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.ProductsParameters;

namespace Contracts.ServiceContracts
{
    public interface ICartService
    {
        Task AddProduct(Guid productId, string userName);
        Task DeleteProductFromCart(Guid productId, string userName);
        Task<(CartDto cart, PagginationMetaData metaData)> GetCartByUserName(string userName, ProductsParameters parameters);
        Task<bool> CheckProductInCart(Guid productId, string userName);
    }
}
