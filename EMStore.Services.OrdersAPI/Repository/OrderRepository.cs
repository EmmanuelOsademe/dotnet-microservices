using EMStore.Services.OrdersAPI.Data;
using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Mappers;
using EMStore.Services.OrdersAPI.Repository.Interfaces;
using EMStore.Services.OrdersAPI.Utility;

namespace EMStore.Services.OrdersAPI.Repository
{
    public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
    {

        private readonly ApplicationDbContext _dbContext = dbContext;
        public async Task<OrderHeaderDto> CreateOrderAsync(CartDto cartDto)
        {
            var orderHeaderDto = cartDto.CartHeader.ToOrderHeaderDtoFromCartHeaderDto();
            var orderHeader = orderHeaderDto.ToOrderHeaderFromOrderHeaderDto();
            orderHeader.OrderTime = DateTime.Now;
            orderHeader.Status = StaticDetails.Status_Pending;

            var orderHeaderCreated = await _dbContext.OrderHeaders.AddAsync(orderHeader);
            await _dbContext.SaveChangesAsync();

            orderHeaderDto.OrderTime = orderHeaderCreated.Entity.OrderTime;
            orderHeaderDto.Status = StaticDetails.Status_Pending;
            orderHeaderDto.OrderHeaderId = orderHeaderCreated.Entity.OrderHeaderId;

            return orderHeaderDto;
        }
    }
}
