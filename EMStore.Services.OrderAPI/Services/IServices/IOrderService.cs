using EMStore.Services.OrderAPI.Dtos;

namespace EMStore.Services.OrderAPI.Services.IServices
{
    public interface IOrderService
    {
        Task<OrderHeaderDto> CreateOrderAsync(CartDto cartDto);
    }
}
