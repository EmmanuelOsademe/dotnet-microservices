using EMStore.Services.OrdersAPI.Dtos;

namespace EMStore.Services.OrdersAPI.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderHeaderDto> CreateOrderAsync(CartDto cartDto);
    }
}
