using AspNetCore.Identity.Dapper.Models;
using Contracts.RepositoryContracts;
using Contracts.ServiceContracts;
using Entities.Exceptions;
using LoggerService;
using Microsoft.AspNetCore.Identity;
using Shared.Dto.CartDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.ProductsParameters;

namespace Service
{
    public class CartService : ICartService
    {
        readonly IRepositoryManager _repo;
        readonly ILoggerManager _logger;
        readonly UserManager<ApplicationUser> _userManager;
        public CartService(IRepositoryManager repo, ILoggerManager logger, UserManager<ApplicationUser> user)
        {
            _repo = repo;
            _logger = logger;
            _userManager = user;
        }

        public async Task AddProduct(Guid productId, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName)
                ?? throw new NotFoundException("user", userName);

            await _repo.CartRepo.AddProduct(user.Id, productId);
        }

        public async Task DeleteProductFromCart(Guid productId, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName)
                ?? throw new NotFoundException("user", userName);

            await _repo.CartRepo.DeleteProductFromCart(user.Id, productId);
        }

        public async Task<(CartDto cart, PagginationMetaData metaData)> GetCartByUserId(string userId, ProductsParameters parameters)
        {
            var products = await _repo.ProductRepo.GetProductsByCart(userId, parameters);
            
            return (cart: new CartDto()
            {
                UserId = userId,
                products = products.pagedResult,
                SummaryPrice = products.summaryPrice,
            }, metaData: products.pagedResult.MetaData);
        }

        public async Task<(CartDto cart, PagginationMetaData metaData)> GetCartByUserName(string userName, ProductsParameters parameters)
        {
            var user = await _userManager.FindByNameAsync(userName)
                ?? throw new NotFoundException("user", userName);
            return await GetCartByUserId(user.Id, parameters);
        }

        public async Task<bool> CheckProductInCart(Guid productId, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName)
                ?? throw new NotFoundException("user", userName);

            return await _repo.CartRepo.CheckProductInCart(user.Id, productId);
        }
    }
}
