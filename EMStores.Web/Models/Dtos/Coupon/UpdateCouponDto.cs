using System.ComponentModel.DataAnnotations;

namespace EMStores.Web.Models.Dtos.Coupon
{
    public class UpdateCouponDto
    {
        [Required]
        public string CouponCode { get; set; } = string.Empty;

        [Required]
        public double DiscountAmount { get; set; }

        [Required]
        public int MinAmount { get; set; }
    }
}
