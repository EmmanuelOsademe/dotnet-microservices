using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMStore.Services.OrdersAPI.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderHeaderDto> CreateOrderAsync(CartDto cartDto);
        Task<IEnumerable<OrderDetailsDto>> GetDetailsAsync(int orderHeaderId);
        Task <OrderHeaderDto?> UpdateOrderHeaderAsync(OrderHeaderUpdateDto updateDto);

        Task<OrderHeader> GetOrderByOrderIdAsync(int orderHeaderId);

        Task<List<OrderDto>> GetUserOrdersAsync(string userId);

        Task<List<OrderDto>> GetOrdersAsAdminAsync();

        Task<OrderDto> GetOrderByIdAsync(int orderHeaderId);
    }
}
