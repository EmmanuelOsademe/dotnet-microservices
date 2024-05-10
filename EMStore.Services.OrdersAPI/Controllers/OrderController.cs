﻿using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Repository.Interfaces;
using EMStore.Services.OrdersAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;

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
                    Session session = service.Get(orderHeader.StripeSessionId);

                    var paymentIntentService = new PaymentIntentService();
                    var paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                    if(paymentIntent.Status == "succeeded")
                    {
                        var updateDto = new OrderHeaderUpdateDto
                        {
                            OrderHeaderId = orderHeaderId,
                            PaymentIntentId = session.PaymentIntentId,
                            Status = StaticDetails.Status_Approved
                        };

                        var orderHeaderDto = await _orderRepository.UpdateOrderHeaderAsync(updateDto);
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
    }
}
