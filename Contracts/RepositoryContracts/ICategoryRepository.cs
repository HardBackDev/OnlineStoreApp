using Shared.Dto.CategoryDtos;

namespace Contracts.RepositoryContracts
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetCategories();
        Task<CategoryDto> GetCategoryByName(string categoryName);

    }
}
