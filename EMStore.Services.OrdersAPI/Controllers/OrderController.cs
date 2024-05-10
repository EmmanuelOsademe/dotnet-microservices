using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;

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

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<IActionResult> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {
                var orderDetails = await _orderRepository.GetDetailsAsync(stripeRequestDto.OrderHeader.OrderHeaderId);
                List<SessionLineItemOptions> orderItems = [];
                foreach (var item in orderDetails)
                {
                    orderItems.Add(new SessionLineItemOptions
                    {

                    });
                }
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    LineItems = orderItems,
                    Mode = "payment",
                };
                
                var service = new Stripe.Checkout.SessionService();
                service.Create(options);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message ?? "Error creating stripe session";
                response.IsSuccess = false;
                return BadRequest(response);
            }
        } 
    }
}
