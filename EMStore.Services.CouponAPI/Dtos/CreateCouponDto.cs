using System.ComponentModel.DataAnnotations;

namespace EMStore.Services.CouponAPI.Dtos
{
	public class CreateCouponDto
	{

		[Required]
		public string CouponCode { get; set; } = string.Empty;

		[Required]
		public double DiscountAmount { get; set; }

		[Required]
		public int MinAmount { get; set; }
	}
}
