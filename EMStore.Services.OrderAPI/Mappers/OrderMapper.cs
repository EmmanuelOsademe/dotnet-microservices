using EMStore.Services.OrderAPI.Dtos;
using EMStore.Services.OrderAPI.Models;

namespace EMStore.Services.OrderAPI.Mappers
{
    public static class OrderMapper
    {
        public static OrderHeaderDto ToOrderHeaderDtoFromCartHeaderDto(this CartHeaderDto headerDto)
        {
            var orderHeader = new OrderHeaderDto
            {
                UserId = headerDto.UserId,
                CouponCode = headerDto.CouponCode,
                Discount = headerDto.Discount,
                OrderTotal = headerDto.CartTotal,
                Name = headerDto.Name,
                Phone = headerDto.Phone,
                Email = headerDto.Email,
            };

            var orderDetails = headerDto.CartDetails.Select(c => c.ToOrderDetailsDtoFromCartDetailsDto());


            return orderHeader;
        }

        public static OrderDetailsDto ToOrderDetailsDtoFromCartDetailsDto(this CartDetailsDto cartDetailsDto)
        {
            var orderDetails = new OrderDetailsDto
            {
                ProductId = cartDetailsDto.ProductId,
                Count =cartDetailsDto.Count,
                ProductName = cartDetailsDto.Product.Name,
                Price = cartDetailsDto.Product.Price,
                Product = cartDetailsDto.Product
            };

            return orderDetails;
        }

        public static CartHeaderDto ToCartHeaderDtoFromOrderHeaderDto(this OrderHeaderDto orderHeaderDto)
        {
            var cartHeaderDto = new CartHeaderDto
            {
                UserId= orderHeaderDto.UserId,
                CouponCode= orderHeaderDto.CouponCode,
                Discount= orderHeaderDto.Discount,
                CartTotal = orderHeaderDto.OrderTotal,
                Name = orderHeaderDto.Name,
                Phone = orderHeaderDto.Phone,
                Email = orderHeaderDto.Email
            };

            return cartHeaderDto;
        }

        public static CartDetailsDto ToCartDetailsDtoFromOrderDetailDto(this OrderDetailsDto orderDetails)
        {
            return new CartDetailsDto
            {
                ProductId = orderDetails.ProductId,
                Product = orderDetails.Product, 
                Count = orderDetails.Count,
            };
        }

        public static OrderHeader ToOrderHeaderFromOrderHeaderDto(this OrderHeaderDto headerDto)
        {
            var order = new OrderHeader
            {
                UserId = headerDto.UserId,
                CouponCode = headerDto.CouponCode,
                Discount = headerDto.Discount,
                OrderTotal = headerDto.OrderTotal,
                Name= headerDto.Name,
                Phone = headerDto.Phone,
                Email = headerDto.Email,
                OrderTime = headerDto.OrderTime,
                OrderDetails = headerDto.OrderDetails
            };

            return order;
        }

        public static OrderHeaderDto ToOrderHeaderDtoFromOrderHeader(this OrderHeader header)
        {
            var order = new OrderHeaderDto
            {
                OrderHeaderId = header.OrderHeaderId,
                UserId = header.UserId,
                CouponCode = header.CouponCode,
                Discount = header.Discount,
                OrderTotal = header.OrderTotal,
                Name = header.Name,
                Phone = header.Phone,
                Email = header.Email,
                OrderTime = header.OrderTime,
                Status = header.Status,
                PaymentIntentId = header.PaymentIntentId,
                StripeSessionId = header.StripeSessionId
            };

            return order;
        }

        public static OrderDetails ToOrderDetailsFromOrderDetailsDto(this OrderDetailsDto detailsDto)
        {
            return new OrderDetails
            {
                ProductId = detailsDto.ProductId,
                Count = detailsDto.Count,
                ProductName = detailsDto.ProductName,
                Price = detailsDto.Price,
            };
        }

        public static OrderDetailsDto ToOrderDetailsDtoFromOrderDetails(this OrderDetails details)
        {
            return new OrderDetailsDto
            {
                ProductId = details.ProductId,
                Count = details.Count,
                ProductName = details.ProductName,
                Price = details.Price,
                OrderDetailsId = details.OrderDetailsId,
                OrderHeaderId = details.OrderHeaderId
            };
        }
    }
}
