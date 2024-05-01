using System.ComponentModel.DataAnnotations;

namespace EMStores.Web.Models.Dtos.Cart
{
    public class CartHeaderInputDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        public string CouponCode { get; set; } = string.Empty;

        public double Discount { get; set; }

        public double CartTotal { get; set; }
    }
}
