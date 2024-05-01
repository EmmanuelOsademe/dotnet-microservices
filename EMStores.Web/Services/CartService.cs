using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Models.Dtos.Cart;
using EMStores.Web.Services.IServices;
using EMStores.Web.Utility;
using Microsoft.AspNetCore.Components.Forms;

namespace EMStores.Web.Services
{
    public class CartService (IBaseService baseService) : ICartService
    {
        private readonly IBaseService _baseService = baseService;
        public async Task<ResponseDto?> ApplyCouponAsync(CartInputDto inputDto)
        {
            RequestDto requestDto = new()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseUrl + "/api/cart/ApplyCoupon",
                Data = inputDto
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto?> EmailCartAsync(CartDto cartDto)
        {
            RequestDto requestDto = new()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseUrl + "/api/cart/EmailCartRequest",
                Data = cartDto
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
        {
            RequestDto requestDto = new()
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseUrl + $"/api/cart/GetCart/{userId}",
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto?> RemoveCouponAsync(CartInputDto inputDto)
        {
            RequestDto requestDto = new()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseUrl + "/api/cart/RemoveCoupon",
                Data = inputDto
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(RemoveCartDto inputDto)
        {
            RequestDto requestDto = new()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseUrl + "/api/cart/RemoveCart",
                Data = inputDto
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartInputDto inputDto)
        {
            RequestDto requestDto = new()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseUrl + "/api/cart/CartUpsert",
                Data = inputDto
            };

            return await _baseService.SendAsync(requestDto);
        }
    }
}
