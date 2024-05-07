

using EMStore.Services.OrderAPI.Models;

namespace EMStore.Services.OrderAPI.Dtos
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string CouponCode { get; set; } = string.Empty;


        public double Discount { get; set; }

 
        public double CartTotal { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Phone { get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;

        public IEnumerable<CartDetailsDto> CartDetails { get; set; }

    }
}
