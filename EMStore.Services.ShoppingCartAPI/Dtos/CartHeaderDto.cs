using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMStore.Services.ShoppingCartAPI.Dtos
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public string CouponCode { get; set; } = string.Empty;


        public double Discount { get; set; }

 
        public double CartTotal { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Phone { get; set;} = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
