
using Contracts.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.RequestFeatures.ProductsParameters;
using System.Text.Json;

namespace OnlineStoreServer.Presentation.Cotrollers
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]

    public class CartController : ControllerBase
    {
        readonly IServiceManager _service;
        public CartController(IServiceManager service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetUserCartProducts([FromQuery] ProductsParameters parameters)
        {
            var pagedResult = await _service.CartService.GetCartByUserName(User.Identity.Name, parameters);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.cart);
        }

        [Authorize]
        [HttpGet("checkInCart/{productId:Guid}")]
        public async Task<IActionResult> CheckProductInCart(Guid productId)
        {
            return Ok(await _service.CartService.CheckProductInCart(productId, User.Identity.Name));
        }

        [Authorize]
        [HttpPost("{id:Guid}")]
        public async Task<IActionResult> AddProductToUserCart(Guid id)
        {
            await _service.CartService.AddProduct(id, User.Identity.Name);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteProductFromCart(Guid id)
        {
            await _service.CartService.DeleteProductFromCart(id, User.Identity.Name);
            return Ok();
        }
    }
}
