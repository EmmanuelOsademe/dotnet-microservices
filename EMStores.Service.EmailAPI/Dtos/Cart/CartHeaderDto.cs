using System.ComponentModel.DataAnnotations;

namespace EMStore.Services.EmailAPI.Dtos.Cart
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public string CouponCode { get; set; } = string.Empty;


        public double Discount { get; set; }

 
        public double CartTotal { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
