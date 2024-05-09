using EMStore.Services.OrderAPI.Dtos;
using EMStore.Services.OrderAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMStore.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ResponseDto response = new();

        //[Authorize]
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CartDto cartDto)
        {
            Console.WriteLine(cartDto);
            try
            {
                OrderHeaderDto headerDto = await _orderService.CreateOrderAsync(cartDto);
                response.Result = headerDto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message ?? "An error occurred";
                return BadRequest(response);
            }

        }

        ////[Authorize]
        public async Task<IActionResult> Testing()
        {
            response.IsSuccess = true;
            response.Message = "Testing";

            return Ok(response);
        }

    }
}
