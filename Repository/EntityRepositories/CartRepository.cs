using Contracts.RepositoryContracts;
using Dapper;
using Entities.Models;
using Repository.Queries;
using Shared.Dto.CartDtos;
using Shared.Dtos.ProductDtos;

namespace Repository.EntityRepositories
{
    public class CartRepository : ICartRepository
    {
        readonly DapperContext _context;

        public CartRepository(DapperContext dapperContext)
        {
            _context = dapperContext;
        }

        public async Task AddProduct(string userId, Guid productId)
        {
            string query = CartQuery.AddProductToCartQuery;
            
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new {ProductId = productId, UserId = userId});
        }

        public async Task DeleteProductFromCart(string userId, Guid productId)
        {
            string query = CartQuery.DeleteProductFromCart;

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { ProductId = productId, UserId = userId });
        }

        public async Task<bool> CheckProductInCart(string userId, Guid productId)
        {
            string query = ProductQuery.CheckProductInCart();

            using var connection = _context.CreateConnection();
            var product = await connection.QuerySingleOrDefaultAsync(query, new { productId, userId });

            return product is not null;
        }
    }
}
