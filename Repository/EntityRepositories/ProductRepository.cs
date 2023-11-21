using Contracts.RepositoryContracts;
using Dapper;
using Entities.Exceptions;
using Repository.Queries;
using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures;
using Shared.RequestFeatures.DtoAttributes;
using Shared.RequestFeatures.ProductsParameters;
using Shared.utilities;
using System.Text.Json;

namespace Repository.EntityRepositories
{
    public class ProductRepository : IProductRepository
    {
        readonly DapperContext _context;

        public ProductRepository(DapperContext dapperContext)
        {
            _context = dapperContext;
        }

        public async Task<PagedList<ProductDto>> GetAllProductByCategory(string category, ProductsParameters parameters)
        {
            string query = ProductQuery.GetSelectProductsByCategory(category, parameters);
            var param = new DynamicParameters(parameters);

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(query, param);
            
            var count = await multi.ReadSingleAsync<int>();
            var products = (await multi.ReadAsync<ProductDto>()).ToList();

            return new PagedList<ProductDto>(products, count, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<(ProductDto, Dictionary<string, string>)> GetProductById(string category, Guid productId)
        {
            string query = ProductQuery.SelectProductById(category);

            using var connection = _context.CreateConnection();
            object productDetails = await connection.QuerySingleOrDefaultAsync(query, new { Id = productId });
            var productType = Type.GetType($"Shared.Dto.ProductCategoriesDtos.{category}Dtos.{category}Dto, Shared", false) ??
                throw new BadRequestException($"The searched type for manupulating {category} not found. " +
                $"searched for the path \"Shared.Dto.ProductCategoriesDtos.{category}Dtos.{category}Dtos\""); ;

            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new DateOnlyJsonConverter());
            string json = JsonSerializer.Serialize(productDetails);
            ProductDto product = JsonSerializer.Deserialize(json, productType, jsonOptions) as ProductDto;
            var propertiesNames = GetProductPropertiesNames(productType);

            return (product, propertiesNames);
        }

        public async Task<(PagedList<ProductDto> pagedResult, long summaryPrice)> GetProductsByCart(string userId, ProductsParameters parameters)
        {
            string query = ProductQuery.GetSelectProductsByCartId(parameters);
            var param = new DynamicParameters(parameters);
            param.Add("Id", userId);

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(query, param);

            var count = await multi.ReadSingleAsync<int>();
            var summaryPrice = await multi.ReadSingleAsync<long>();
            var products = (await multi.ReadAsync<ProductDto>()).ToList();

            return (pagedResult: new PagedList<ProductDto>(products, count, parameters.PageNumber, parameters.PageSize), summaryPrice: summaryPrice);
        }

        public async Task<Guid> CreateProduct(string category, ProductForManipulating product)
        {
            string query = ProductQuery.GetInsertProduct(category, product);
            var param = new DynamicParameters(product);
            using var connection = _context.CreateConnection();
            var createdId = await connection.QuerySingleAsync<Guid>(query, param);
            return createdId;
        }

        public async Task UpdateProduct(string category, ProductForManipulating product, Guid id)
        {
            string query = ProductQuery.GetUpdateProduct(category, product);
            var param = new DynamicParameters(product);
            param.Add("Id", id);
            using var connection = _context.CreateConnection();
            await connection.QueryAsync(query, param);
        }

        public async Task DeleteProduct(string category, Guid id)
        {
            string query = ProductQuery.DeleteProduct(category);
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new {Id = id});
        }

        static Dictionary<Type, Dictionary<string, string>> productPropertiesNamesCache = new();
        private Dictionary<string, string> GetProductPropertiesNames(Type productType)
        {
            if(productPropertiesNamesCache.TryGetValue(productType, out var productPropertiesNames))
                return productPropertiesNames;

            var propertiesNames = new Dictionary<string, string>();
            foreach (var property in productType.GetProperties())
            {
                if (property.IsDefined(typeof(PropertyNameAttribute), false))
                {
                    var attribute = Attribute.GetCustomAttribute(property, typeof(PropertyNameAttribute)) as PropertyNameAttribute;
                    propertiesNames.Add(property.Name.ToUpper(), attribute.Name);
                }
            }
            productPropertiesNamesCache.Add(productType, propertiesNames);
            return propertiesNames;
        }
    }
}
