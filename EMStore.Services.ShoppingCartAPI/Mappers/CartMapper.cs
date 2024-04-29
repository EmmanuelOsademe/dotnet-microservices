using EMStore.Services.ShoppingCartAPI.Dtos;
using EMStore.Services.ShoppingCartAPI.Models;

namespace EMStore.Services.ShoppingCartAPI.Mappers
{
    public static class CartMapper
    {

        public static CartHeader ToCartHeaderFromCartHeaderDto(this CartHeaderDto dto)
        {
            CartHeader header = new();
            //return new CartHeader
            //{
            //    CartHeaderId = dto.CartHeaderId,
            //    UserId = dto.UserId,
            //    CouponCode = dto.CouponCode,
            //    Discount = dto.Discount,
            //    CartTotal = dto.CartTotal
            //};
            if(dto != null)
            {
                header.UserId = dto.UserId;
                header.CouponCode = dto.CouponCode;
                header.Discount = dto.Discount;
                header.CartTotal = dto.CartTotal;
            }
            
            return header;
        }

        public static CartHeaderDto ToDtoFromCartHeader(this CartHeader header)
        {
            return new CartHeaderDto
            {
                CartHeaderId = header.CartHeaderId,
                UserId = header.UserId,
                CouponCode = header.CouponCode,
                Discount = header.Discount,
                CartTotal = header.CartTotal
            };
        }

        public static CartDetails ToCartDetailsFromCartDetailsDto(this CartDetailsDto dto)
        {
            return new CartDetails
            {
                CartDetailsId = dto.CartDetailsId,
                CartHeaderId = dto.CartHeaderId,
                CartHeader = dto.CartHeader.ToCartHeaderFromCartHeaderDto(),
                ProductId = dto.ProductId,
                Product = dto.Product,
                Count = dto.Count
            };
        }

        public static CartDetailsDto ToDtoFromCartDetails(this CartDetails details)
        {
            return new CartDetailsDto
            {
                CartDetailsId = details.CartDetailsId,
                CartHeaderId = details.CartHeaderId,
                CartHeader = details.CartHeader.ToDtoFromCartHeader(),
                ProductId = details.ProductId,
                Product = details.Product,
                Count = details.Count
            };
        }

        public static CartDto ToCartDtoFromCartInputDto(this CartInputDto inputDto)
        {

            CartHeaderDto cartHeader = new()
            {
                UserId = inputDto.CartHeaderInputDto.UserId,
                CouponCode = inputDto.CartHeaderInputDto.CouponCode,
                Discount = inputDto.CartHeaderInputDto.Discount,
                CartTotal = inputDto.CartHeaderInputDto.CartTotal
            };

            List<CartDetailsDto> cartDetails = [
                new(){
                    ProductId = inputDto.CartDetailsInputDto.ProductId,
                    Count = inputDto.CartDetailsInputDto.Count
                }
                ];

            return new CartDto
            {
                CartHeader = cartHeader,
                CartDetails = cartDetails
            };
        }
    }
}
