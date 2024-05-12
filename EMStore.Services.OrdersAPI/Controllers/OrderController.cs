using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Repository.Interfaces;
using EMStore.Services.OrdersAPI.Utility;
using EMStores.MessageBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;

namespace EMStore.Services.OrdersAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController(IOrderRepository orderRepository, IMessageBus messageBus, IConfiguration config) : ControllerBase
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IConfiguration _config = config;
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
                // Create session items
                var orderDetails = await _orderRepository.GetDetailsAsync(stripeRequestDto.OrderHeader.OrderHeaderId);
                List<SessionLineItemOptions> orderItems = [];
                foreach (var item in orderDetails)
                {
                    orderItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // $20.99 -> 2099
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductName
                            }
                        },
                        Quantity = item.Count
                    });
                }


                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    LineItems = orderItems,
                    Mode = "payment",
                };

                if(stripeRequestDto.OrderHeader.Discount > 0)
                {
                    var Discounts = new List<SessionDiscountOptions>()
                    {
                        new SessionDiscountOptions
                        {
                            Coupon = stripeRequestDto.OrderHeader.CouponCode
                        }
                    };

                    options.Discounts = Discounts;
                }

                
                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);

                stripeRequestDto.StripeSessionUrl = session.Url;

                // Save stripeSessionID to the DB
                var updateDto = new OrderHeaderUpdateDto
                {
                    OrderHeaderId = stripeRequestDto.OrderHeader.OrderHeaderId,
                    StripeSessionId = session.Id
                };

                await _orderRepository.UpdateOrderHeaderAsync(updateDto);
                
                response.Result = stripeRequestDto;
               
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message ?? "Error creating stripe session";
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<IActionResult> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                var orderHeader = await _orderRepository.GetOrderByOrderIdAsync(orderHeaderId);

                var service = new Stripe.Checkout.SessionService();
                if(orderHeader != null)
                {
                    // Validate Successful payment
                    Session session = service.Get(orderHeader.StripeSessionId);

                    var paymentIntentService = new PaymentIntentService();
                    var paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                    if(paymentIntent.Status == "succeeded")
                    {
                        // Update Order Header
                        var updateDto = new OrderHeaderUpdateDto
                        {
                            OrderHeaderId = orderHeaderId,
                            PaymentIntentId = session.PaymentIntentId,
                            Status = StaticDetails.Status_Approved
                        };

                        var orderHeaderDto = await _orderRepository.UpdateOrderHeaderAsync(updateDto);

                        // Publish successful order to service bus
                        RewardsDto rewardsDto = new()
                        {
                            OrderId = orderHeader.OrderHeaderId,
                            UserId = orderHeader.UserId,
                            RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal)
                        };
                        string topicName = _config.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic") ?? string.Empty;
                        string serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString") ?? string.Empty;
                        await _messageBus.PublishMessage(rewardsDto, topicName, serviceBusConnectionString);
                        response.Result = orderHeaderDto;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message ?? "Error creating stripe session";
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders([FromRoute] string? userId = "")
        {
            try
            {
                List<OrderDto> orders = new();
                if (User.IsInRole(StaticDetails.RoleAdmin))
                {
                    orders = await _orderRepository.GetOrdersAsAdminAsync();
                }else
                {
                    if (string.IsNullOrEmpty(userId))
                    {
                        throw new Exception("User Id is required");
                    }
                    orders = await _orderRepository.GetUserOrdersAsync(userId);
                }

                response.Result = orders;
                response.IsSuccess = true;
                return Ok(response);
            }catch(Exception ex)
            {
                response.Message = ex.Message ?? "Error creating stripe session";
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpGet("GetOrder/{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if(order == null)
                {
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                }
                else
                {
                    response.Result = order;
                    response.IsSuccess = true;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message ?? "Error creating stripe session";
                response.IsSuccess = false;
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                var updateDto = new OrderHeaderUpdateDto
                {
                    OrderHeaderId = orderId,
                    Status = newStatus
                };

                var orderHeaderDto = await _orderRepository.UpdateOrderHeaderAsync(updateDto) ?? throw new Exception("Order not found");

                if(newStatus == StaticDetails.Status_Cancelled)
                {
                    // Give a refund
                    var options = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderHeaderDto.PaymentIntentId
                    };

                    var service = new RefundService();
                    Refund refund = service.Create(options);
                }
                
                response.Result = orderHeaderDto;
                response.IsSuccess = true;
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
