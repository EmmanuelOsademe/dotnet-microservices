using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Models;

namespace EMStore.Services.OrdersAPI.Mappers
{
    public static class OrderMapper
    {
        public static OrderHeaderDto ToOrderHeaderDtoFromCartHeaderDto(this CartHeaderDto cartHeaderDto)
        {

            return new OrderHeaderDto
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

        public static OrderHeader ToOrderHeaderFromOrderHeaderDto(this OrderHeaderDto headerDto)
        {
            return new OrderHeader
            {
                UserId = headerDto.UserId,
                CouponCode = headerDto.CouponCode,
                Discount = headerDto.Discount,
                OrderTotal = headerDto.OrderTotal,
                Name = headerDto.Name,
                Phone = headerDto.Phone,
                Email = headerDto.Email,
                OrderTime = DateTime.Now
            };
        }
    }
}
