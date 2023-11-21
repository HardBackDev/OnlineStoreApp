using Contracts.RepositoryContracts;
using Entities.Models;
using Repository.EntityRepositories;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        readonly Lazy<IProductRepository> _productRepo;
        readonly Lazy<ICartRepository> _cartRepo;
        readonly Lazy<ICategoryRepository> _categoryRepo;

        public RepositoryManager(DapperContext context)
        {
            _productRepo = new Lazy<IProductRepository>(() => new ProductRepository(context));
            _cartRepo = new Lazy<ICartRepository>(() => new CartRepository(context));
            _categoryRepo = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));
        }
        
        public IProductRepository ProductRepo => _productRepo.Value;
        public ICartRepository CartRepo => _cartRepo.Value;
        public ICategoryRepository CategoryRepo => _categoryRepo.Value;
    }
}
