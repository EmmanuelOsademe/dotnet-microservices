using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Models;

namespace EMStore.Services.OrdersAPI.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderHeaderDto> CreateOrderAsync(CartDto cartDto);
        Task<IEnumerable<OrderDetailsDto>> GetDetailsAsync(int orderHeaderId);
        Task <OrderHeaderDto?> UpdateOrderHeaderAsync(OrderHeaderUpdateDto updateDto);

        Task<OrderHeader> GetOrderByOrderIdAsync(int orderHeaderId);
    }
}
