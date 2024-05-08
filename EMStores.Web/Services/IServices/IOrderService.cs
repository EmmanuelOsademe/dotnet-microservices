using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;

namespace EMStores.Web.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDto?> CreateOrderAsync(CartDto cartDto);
    }
}
