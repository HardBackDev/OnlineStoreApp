using Contracts.RepositoryContracts;
using Dapper;
using Repository.Queries;
using Shared.Dto.CategoryDtos;

namespace Repository.EntityRepositories
{
    public class CategoryRepository : ICategoryRepository
    {
        readonly DapperContext _context;

        public CategoryRepository(DapperContext dapperContext)
        {
            _context = dapperContext;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            string query = CategoryQuery.selectCategoriesByCatalogQuery;
            using var connection = _context.CreateConnection();
            var categories = await connection.QueryAsync<CategoryDto>(query);
            return categories;
        }

        public async Task<CategoryDto> GetCategoryByName(string categoryName)
        {
            string query = CategoryQuery.selectCategoryByNameQuery;
            using var connection = _context.CreateConnection();
            var category = await connection.QuerySingleOrDefaultAsync<CategoryDto>(query, new { Name = categoryName });
            return category;
        }
    }
}
