using EMStore.Services.ShoppingCartAPI.Dtos;
using EMStore.Services.ShoppingCartAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMStore.Services.ShoppingCartAPI.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartAPIController(ICartRepository cartRepository) : ControllerBase
    {
        private readonly ResponseDto response = new();
        private readonly ICartRepository _cartRespository = cartRepository;

        [HttpPost("CartUpsert")]
        public async Task<IActionResult> CartUpsert([FromBody] CartInputDto cartDto)
        {
            try
            {
                var responseDto = await _cartRespository.UpsertCartAsync(cartDto);
                response.Result = responseDto;
                response.Message = "Cart saved successfully";
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message.ToString() ?? "Error Upserting cart";
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }
    }
}
