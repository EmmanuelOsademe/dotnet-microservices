using EMStore.Services.OrderAPI.Data;
using EMStore.Services.OrderAPI.Dtos;
using EMStore.Services.OrderAPI.Mappers;
using EMStore.Services.OrderAPI.Models;
using EMStore.Services.OrderAPI.Services.IServices;
using EMStore.Services.OrderAPI.Utility;

namespace EMStore.Services.OrderAPI.Services
{
    public class OrderService(ApplicationDbContext dbContext) : IOrderService
    {

        private readonly ApplicationDbContext _dbContext = dbContext;
        public async Task<OrderHeaderDto> CreateOrderAsync(CartDto cartDto)
        {
            // Create the orderHeader
            OrderHeaderDto headerDto = cartDto.CartHeader.ToOrderHeaderDtoFromCartHeaderDto();
            headerDto.OrderTime = DateTime.Now;
            headerDto.Status = StaticDetails.Status_Pending;

            OrderHeader header = headerDto.ToOrderHeaderFromOrderHeaderDto();

            var orderHeaderCreated = await _dbContext.OrderHeaders.AddAsync(header);
            await _dbContext.SaveChangesAsync();

            headerDto.OrderHeaderId = orderHeaderCreated.Entity.OrderHeaderId;
            return headerDto;
        }
    }
}
