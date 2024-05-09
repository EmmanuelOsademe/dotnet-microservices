using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMStore.Services.OrdersAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController(IOrderRepository orderRepository) : ControllerBase
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ResponseDto response = new();

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                var headerDto = await _orderRepository.CreateOrderAsync(cartDto);
                response.Result = headerDto;
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message ?? "Error creating order";
                return BadRequest(response);
            }
        }
    }
}
