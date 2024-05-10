using EMStore.Services.OrdersAPI.Data;
using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Mappers;
using EMStore.Services.OrdersAPI.Models;
using EMStore.Services.OrdersAPI.Repository.Interfaces;
using EMStore.Services.OrdersAPI.Utility;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.OrdersAPI.Repository
{
    public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
    {

        private readonly ApplicationDbContext _dbContext = dbContext;
        public async Task<OrderHeaderDto> CreateOrderAsync(CartDto cartDto)
        {
            OrderHeader orderHeader = cartDto.CartHeader.ToOrderHeaderFromCartHeaderDto();
            orderHeader.OrderTime = DateTime.Now;
            orderHeader.Status = StaticDetails.Status_Pending;

            var orderHeaderCreated = await _dbContext.OrderHeaders.AddAsync(orderHeader);
            await _dbContext.SaveChangesAsync();

            List<OrderDetails> orderDetails = [];

            foreach(var item in cartDto.CartDetails)
            {
                OrderDetails detail = new()
                {
                    Price = item.Product.Price,
                    ProductName = item.Product.Name,
                    Count = item.Count,
                    ProductId = item.ProductId,
                    OrderHeaderId = orderHeaderCreated.Entity.OrderHeaderId
                };
                orderDetails.Add(detail);
            }

            await _dbContext.OrderDetails.AddRangeAsync(orderDetails);
            await _dbContext.SaveChangesAsync();

            var orderHeaderDto = orderHeaderCreated.Entity.ToOrderHeaderDtoFromOrderHeader();

            return orderHeaderDto;
        }

        public async  Task<IEnumerable<OrderDetailsDto>> GetDetailsAsync(int orderHeaderId)
        {
            var orderDetails = await _dbContext.OrderDetails.Where(order => order.OrderHeaderId == orderHeaderId).ToListAsync();
            List<OrderDetailsDto> detailsDto = [];
            foreach(var details in  orderDetails)
            {
                OrderDetailsDto detail = new()
                {
                    OrderDetailsId = details.OrderDetailsId,
                    OrderHeaderId = details.OrderHeaderId,
                    ProductId = details.ProductId,
                    ProductName = details.ProductName,
                    Price = details.Price,
                    Count = details.Count,
                };
                detailsDto.Add(detail);
            }
            return detailsDto;
        }
    }
}
