using Shared.Dto.CartDtos;

namespace Contracts.RepositoryContracts
{
    public interface ICartRepository
    {
        Task DeleteProductFromCart(string userId, Guid productId);
        Task AddProduct(string userId, Guid productId);
        Task<bool> CheckProductInCart(string userId, Guid productId);
    }
}
