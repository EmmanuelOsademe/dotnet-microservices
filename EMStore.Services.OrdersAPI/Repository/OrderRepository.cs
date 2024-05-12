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

        public async Task<OrderDto> GetOrderByIdAsync(int orderHeaderId)
        {
            var orderHeader = await _dbContext.OrderHeaders.FirstOrDefaultAsync(order => order.OrderHeaderId == orderHeaderId) ?? throw new Exception("Order not found");

            var orderDetails = await _dbContext.OrderDetails.Where(order => order.OrderHeaderId == orderHeaderId).ToListAsync();

            var orderHeaderDto = orderHeader.ToOrderHeaderDtoFromOrderHeader();
            var orderDetailsDto = orderDetails.Select(order => order.ToOrderDetailsDtoFromOrderDetails()).ToList();

            return new OrderDto()
            {
                OrderHeader = orderHeaderDto,
                OrderDetails = orderDetailsDto
            };
        }

        public async Task<OrderHeader> GetOrderByOrderIdAsync(int orderHeaderId)
        {
            OrderHeader orderHeader = await _dbContext.OrderHeaders.FirstAsync(u => u.OrderHeaderId == orderHeaderId);
            return orderHeader;
        }

        public async Task<List<OrderDto>> GetOrdersAsAdminAsync()
        {
            var orderHeaders = await _dbContext.OrderHeaders.OrderByDescending(order => order.OrderHeaderId).ToListAsync();

            List<OrderDto> orders = [];

            foreach (var item in orderHeaders)
            {
                var orderDetails = await _dbContext.OrderDetails.Where(order => order.OrderHeaderId == item.OrderHeaderId).ToListAsync();
                var orderHeaderDto = item.ToOrderHeaderDtoFromOrderHeader();
                var orderDetailsDto = orderDetails.Select(order => order.ToOrderDetailsDtoFromOrderDetails()).ToList();

                orders.Add(new OrderDto
                {
                    OrderHeader = orderHeaderDto,
                    OrderDetails = orderDetailsDto
                });
            }

            return orders;
        }

        public async Task<List<OrderDto>> GetUserOrdersAsync(string userId)
        {
            var orderHeaders = await _dbContext.OrderHeaders.Where(order => order.UserId == userId).OrderByDescending(order => order.OrderHeaderId).ToListAsync();

            List<OrderDto> orders = [];

            foreach(var item in orderHeaders)
            {
                var orderDetails = await _dbContext.OrderDetails.Where(order => order.OrderHeaderId == item.OrderHeaderId).ToListAsync();
                var orderHeaderDto = item.ToOrderHeaderDtoFromOrderHeader();
                var orderDetailsDto = orderDetails.Select(order => order.ToOrderDetailsDtoFromOrderDetails()).ToList();

                orders.Add(new OrderDto
                {
                    OrderHeader = orderHeaderDto,
                    OrderDetails = orderDetailsDto
                });
            }

            return orders;
        }

        public async  Task<OrderHeaderDto?> UpdateOrderHeaderAsync(OrderHeaderUpdateDto updateDto)
        {
            OrderHeader orderHeader = await _dbContext.OrderHeaders.FirstAsync(u => u.OrderHeaderId == updateDto.OrderHeaderId) ?? throw new Exception("Order not found");

            if(!string.IsNullOrEmpty(updateDto.StripeSessionId))
            {
                orderHeader.StripeSessionId = updateDto.StripeSessionId;
            }

            if (!string.IsNullOrEmpty(updateDto.PaymentIntentId))
            {
                orderHeader.PaymentIntentId = updateDto.PaymentIntentId;
            }

            if (!string.IsNullOrEmpty(updateDto.Status))
            {
                orderHeader.Status = updateDto.Status;
            }
            
            await _dbContext.SaveChangesAsync();

            return orderHeader.ToOrderHeaderDtoFromOrderHeader();
        }
    }
}
