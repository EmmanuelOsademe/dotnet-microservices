using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Models.Dtos.Cart;
using EMStores.Web.Services.IServices;
using EMStores.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace EMStores.Web.Controllers
{
    public class CartController (ICartService cartService, IOrderService orderService) : Controller
    {
        private readonly ICartService _cartService = cartService;
        private readonly IOrderService _orderService = orderService;

        [Authorize]
        public async Task <IActionResult> CartIndex()
        {
            CartDto cart = await LoadCartBasedOnLoggedInUser();
            return View(cart);
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            CartDto cart = await LoadCartBasedOnLoggedInUser();
            return View(cart);
        }

        [Authorize]
        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            CartDto cart = await LoadCartBasedOnLoggedInUser();
            cart.CartHeader.Name = cartDto.CartHeader.Name;
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;

            var response = await _orderService.CreateOrderAsync(cart);
            if(response != null && response.IsSuccess)
            {
                OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

                var domain = $"{Request.Scheme}://{Request.Host.Value}/";

                StripeRequestDto stripeRequestDto = new()
                {
                    ApprovedUrl = $"{domain}cart/Confirmation?orderId={orderHeaderDto.OrderHeaderId}",
                    CancelUrl = $"{domain}/cart/Checkout",
                    OrderHeader = orderHeaderDto
                };

                var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
                StripeRequestDto stripeResponseObj = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(stripeResponse.Result));

                Response.Headers.Add("Location", stripeResponseObj.StripeSessionUrl);
                return new StatusCodeResult(303);
            }
            else
            {
                TempData["error"] = "Error placing order";
            }
            
            return View(cart);
        }

        [Authorize]
        public async Task<IActionResult> Confirmation( int orderId)
        {
            ResponseDto? response = await _orderService.ValidateStripeSession(orderId);
            if( response != null && response.IsSuccess )
            {
                OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
                if(orderHeaderDto.Status == StaticDetails.Status_Approved)
                {
                    TempData["success"] = "Payment successful";
                    return View(orderId);
                }
                
            }
            TempData["error"] = "Payment failed";
            // Redirect to the error page
            return View(orderId);
        }


        public async Task<IActionResult> RemoveCartItem(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value ?? string.Empty;
            RemoveCartDto inputDto = new() { UserId = userId, CartDetailsId = cartDetailsId };
            ResponseDto? response = await _cartService.RemoveFromCartAsync(inputDto);
            if(response != null && response.IsSuccess)
            {
                TempData["success"] = "Item removed from cart";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = "Could not remove item from cart";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value ?? string.Empty;
            CartHeaderInputDto header = new() { UserId = userId, CouponCode = cartDto.CartHeader.CouponCode };
            CartDetailsInputDto details = new();

            CartInputDto inputDto = new() { CartHeaderInputDto = header, CartDetailsInputDto = details };

            ResponseDto? response = await _cartService.ApplyCouponAsync(inputDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon code applied successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = "Error applying coupon code";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartBasedOnLoggedInUser();
            var email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value ?? string.Empty;
            cart.CartHeader.Email = email;
            
            ResponseDto? response = await _cartService.EmailCartAsync(cart);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent shortly";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = "Error sending message";
            return RedirectToAction(nameof(CartIndex));
        }


        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value ?? string.Empty;
            CartHeaderInputDto header = new() { UserId = userId,  CouponCode = cartDto.CartHeader.CouponCode };
            CartDetailsInputDto details = new();

            CartInputDto inputDto = new() { CartHeaderInputDto = header, CartDetailsInputDto = details };

            ResponseDto? response = await _cartService.RemoveCouponAsync(inputDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon code removed successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = "Error removing coupon code";
            return View();
        }




        private async Task<CartDto> LoadCartBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value ?? string.Empty;
            ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);
            if(response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto ?? new CartDto ();
            }
            return new CartDto();
        }
    }
}
