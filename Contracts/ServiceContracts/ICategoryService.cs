using Shared.Dto.CategoryDtos;
using Shared.RequestFeatures;

namespace Contracts.ServiceContracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategories();
        ParametersMetaData GetCategoryParametersMetadata(string category);
        ManipulatingDtoMetadata GetManipulatingDtoMetadata(string category);

    }
}
