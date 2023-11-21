using AspNetCore.Identity.Dapper.Models;
using Contracts.RepositoryContracts;
using Contracts.ServiceContracts;
using LoggerService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        readonly Lazy<IProductService> _productService;
        readonly Lazy<IAuthenticationService> _authenticationService;
        readonly Lazy<ICartService> _cartService;
        readonly Lazy<ICategoryService> _categoryService;

        public ServiceManager(IRepositoryManager repo, ILoggerManager logger,
            UserManager<ApplicationUser> userManager, IConfiguration configuration) 
        {
            _productService = new Lazy<IProductService>(() => new ProductService(repo, logger));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, userManager, configuration, repo));
            _cartService = new Lazy<ICartService>(() => new CartService(repo, logger, userManager));
            _categoryService = new Lazy<ICategoryService>(() => new CategoryService(repo));
        }

        public ICartService CartService => _cartService.Value;
        public IProductService ProductService => _productService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public ICategoryService CategoryService => _categoryService.Value;
    }
}
