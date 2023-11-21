
namespace Contracts.ServiceContracts
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IAuthenticationService AuthenticationService { get; }
        ICartService CartService { get; }
        ICategoryService CategoryService { get; }
    }
}
