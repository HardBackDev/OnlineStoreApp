
using Contracts.RepositoryContracts;
using Contracts.ServiceContracts;
using Entities.Exceptions;
using Entities.Models;
using LoggerService;
using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.ProductsParameters;

namespace Service
{
    public class ProductService : IProductService
    {
        readonly IRepositoryManager _repo;
        readonly ILoggerManager _logger;

        public ProductService(IRepositoryManager repo, ILoggerManager logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<PagedList<ProductDto>> GetProductsByCategory(string category, ProductsParameters parameters)
        {
            await CheckCategoryExist(category);
            return await _repo.ProductRepo.GetAllProductByCategory(category, parameters);
        }

        public async Task<(ProductDto, Dictionary<string, string>)> GetProductById(string category, Guid productId)
        {
            await CheckCategoryExist(category);

            return await CheckProductExistAndGet(category, productId);
        }

        public async Task<ProductDto> CreateProduct(string category, ProductForManipulating product)
        {
            await CheckCategoryExist(category);
            var createdId = await _repo.ProductRepo.CreateProduct(category, product);
            return new ProductDto()
            {
                Id = createdId,
                Price = product.Price,
                PhotoUrl = product.PhotoUrl,
                Title = product.Title,
            };
        }

        public async Task DeleteProduct(string category, Guid productId)
        {
            await CheckCategoryExist(category);
            await CheckProductExistAndGet(category, productId);
            await _repo.ProductRepo.DeleteProduct(category, productId);
        }

        public async Task UpdateProduct(string category, ProductForManipulating product, Guid id)
        {
            await CheckCategoryExist(category);
            await CheckProductExistAndGet(category, id);
            await _repo.ProductRepo.UpdateProduct(category, product, id);
        }

        public async Task<(ProductDto, Dictionary<string, string>)> CheckProductExistAndGet(string category, Guid productId)
        {
            var product = await _repo.ProductRepo.GetProductById(category, productId);
            if (product.Item1 is null)
                throw new NotFoundException(category, productId);
            return product;
        }

        private async Task CheckCategoryExist(string category)
        {
            if (!category.Equals("Products", StringComparison.OrdinalIgnoreCase) && await _repo.CategoryRepo.GetCategoryByName(category) == null)
                throw new NotFoundException(nameof(Category), category);
        }

    }
}
