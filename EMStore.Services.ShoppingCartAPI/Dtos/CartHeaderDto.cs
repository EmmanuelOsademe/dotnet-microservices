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

        [Required]
        public double Discount { get; set; }

        [Required]
        public double CartTotal { get; set; }
    }
}
