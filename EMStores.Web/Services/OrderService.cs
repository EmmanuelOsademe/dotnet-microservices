using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Services.IServices;
using EMStores.Web.Utility;

namespace EMStores.Web.Services
{
    public class OrderService (IBaseService baseService) : IOrderService
    {
        private readonly IBaseService _baseService = baseService;
        public async Task<ResponseDto?> CreateOrderAsync(CartDto cartDto)
        {
            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.OrderAPIBaseUrl + "/api/order/CreateOrder",
                Data = cartDto
            };

            return await _baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.OrderAPIBaseUrl + "/api/order/CreateStripeSession",
                Data = stripeRequestDto
            };

            return await _baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> GetAllOrders(string? userId)
        {
            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.OrderAPIBaseUrl + $"/api/order/GetOrders/{userId}",
            };

            return await _baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> GetOrder(int orderId)
        {
            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.OrderAPIBaseUrl + $"/api/order/GetOrder/{orderId}",
            };

            return await _baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
        {
            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.OrderAPIBaseUrl + $"/api/order/UpdateOrderStatus/{orderId}",
                Data = newStatus
            };

            return await _baseService.SendAsync(request);
        }

        public async  Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
        {
            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.OrderAPIBaseUrl + "/api/order/ValidateStripeSession",
                Data = orderHeaderId
            };

            return await _baseService.SendAsync(request);
        }
    }
}
