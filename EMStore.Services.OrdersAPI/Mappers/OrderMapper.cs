using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Models;

namespace EMStore.Services.OrdersAPI.Mappers
{
    public static class OrderMapper
    {
        public static OrderHeader ToOrderHeaderFromCartHeaderDto(this CartHeaderDto cartHeaderDto)
        {

            return new OrderHeader
            {
                UserId = cartHeaderDto.UserId,
                CouponCode = cartHeaderDto.CouponCode,
                Discount = cartHeaderDto.Discount,
                OrderTotal = cartHeaderDto.CartTotal,
                Name = cartHeaderDto.Name,
                Phone = cartHeaderDto.Phone,
                Email = cartHeaderDto.Email,
            };
        }

        public static OrderHeaderDto ToOrderHeaderDtoFromOrderHeader(this OrderHeader header)
        {
            return new OrderHeaderDto
            {
                UserId = header.UserId,
                CouponCode = header.CouponCode,
                Discount = header.Discount,
                OrderTotal = header.OrderTotal,
                Name = header.Name,
                Phone = header.Phone,
                Email = header.Email,
                OrderHeaderId = header.OrderHeaderId,
                Status = header.Status,
                OrderTime = header.OrderTime
            };
        }

        public static OrderDetailsDto ToOrderDetailsDtoFromOrderDetails(this OrderDetails details)
        {
            return new OrderDetailsDto
            {
                OrderDetailsId = details.OrderDetailsId,
                OrderHeaderId = details.OrderHeaderId,
                ProductId = details.ProductId,
                ProductName = details.ProductName,
                Price = details.Price,
                Count = details.Count,
            };
        }

        public static OrderDetails ToOrderDetailsFromOrderDetailsDto(this OrderDetailsDto orderDetailsDto)
        {
            return new OrderDetails
            {
                ProductId = orderDetailsDto.ProductId,
                Count = orderDetailsDto.Count,
                ProductName = orderDetailsDto.ProductName,
                Price = orderDetailsDto.Price
            };
        }
    }
}
