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
    }
}
