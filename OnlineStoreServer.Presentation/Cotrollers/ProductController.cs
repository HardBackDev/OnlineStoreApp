using Contracts.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreServer.Presentation.Filtres;
using OnlineStoreServer.Presentation.ModelBinders;
using Shared.Dtos.ProductDtos;
using Shared.RequestFeatures.ProductsParameters;
using System.Text.Json;

namespace OnlineStoreServer.Presentation.Cotrollers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ProductController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductsParameters parameters)
        {
            var pagedResult = await _service.ProductService.GetProductsByCategory("products", parameters);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.MetaData));
            return Ok(pagedResult);
        }

        [HttpGet("{category}/{id:Guid}", Name = "ProductById")]
        public async Task<IActionResult> GetProductsById(string category, Guid id)
        {
            var product = await _service.ProductService.GetProductById(category, id);
            return Ok(new
            {
                product = product.Item1 as object,
                propertiesNames = product.Item2
            });
        }

        [HttpGet("{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category,
            [ModelBinder(BinderType = typeof(ParametersBinder))] ProductsParameters parameters)
        {
            var pagedResult = await _service.ProductService.GetProductsByCategory(category, parameters);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.MetaData));
            return Ok(pagedResult);
        }

        [Authorize(Roles = "Admin")]
        [ValidationFilter]
        [HttpPost("{category}")]
        public async Task<IActionResult> CreateProduct(string category, [ModelBinder(BinderType = typeof(ManipulatingProductBinder))] ProductForManipulating product)
        {
            var createdProduct = await _service.ProductService.CreateProduct(category, product);

            return CreatedAtRoute("ProductById", new { category, id = createdProduct.Id }, createdProduct);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{category}/{id:Guid}")]
        public async Task<IActionResult> UpdateProduct(string category, Guid id,
            [ModelBinder(BinderType = typeof(ManipulatingProductBinder))] ProductForManipulating product)
        {
            await _service.ProductService.UpdateProduct(category, product, id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{category}/{id:Guid}")]
        public async Task<IActionResult> DeleteProduct(string category, Guid id)
        {
            await _service.ProductService.DeleteProduct(category, id);
            return NoContent();
        }

    }
}
