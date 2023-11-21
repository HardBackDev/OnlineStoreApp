using Contracts.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto.CategoryDtos;
using Shared.RequestFeatures.ParametersAttributes;

namespace OnlineStoreServer.Presentation.Cotrollers
{
    [Route("api/categories")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        readonly IServiceManager _service;
        public CategoryController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _service.CategoryService.GetCategories());
        }

        [HttpGet("{category}/parameters")]
        public async Task<IActionResult> GetCategoryParametersMetadata(string category)
        {
            var parametersMetadata = _service.CategoryService.GetCategoryParametersMetadata(category);

            return Ok(new
            {
                parametersSearchValues = parametersMetadata.ParametersSearchValues,
                parametersNames = parametersMetadata.ParametersNames,
                parameters = parametersMetadata.Parameters,
                orderByColumns = parametersMetadata.OrderByColumns,
                dependentSearchValues = parametersMetadata.DependentSearchValues
            });
        }

        [HttpGet("{category}/manipulatingMetadata")]
        public async Task<IActionResult> GetManipulatingMetadata(string category)
        {
            var manipulatingMetadata = _service.CategoryService.GetManipulatingDtoMetadata(category);
            return Ok(new
            {
                manipulatingObject = manipulatingMetadata.ManipulatinObject,
                propertiesValues = manipulatingMetadata.PropertiesValues,
                onlyDefaultValuesProperties = manipulatingMetadata.OnlyDefaultValuesProperties
            });
        }

    }
}
