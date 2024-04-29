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

        [HttpPost("RemoveCart")]
        public async Task<IActionResult> RemoveCart([FromBody] RemoveCartDto removeCartDto)
        {
            try
            {
                var result = await _cartRespository.RemoveCartAsync(removeCartDto);
                response.Result = result;
                if (!result)
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to removed cart";
                    return Ok(response);
                }
                response.Message = "Cart removed successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString() ?? "Error removing cart";
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<IActionResult> GetCart([FromRoute] string userId)
        {
            try
            {
                var result = await _cartRespository.GetCartByUserIdAsync(userId);
                response.Result = result;                
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString() ?? "Error removing cart";
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }
    }
}
