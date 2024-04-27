using EMStore.Services.CouponAPI.Dtos;
using EMStore.Services.CouponAPI.Models;

namespace EMStore.Services.CouponAPI.Mappers
{
	public static class CouponMapper
	{
		public static CouponDto ToCouponDto(this Coupon coupon)
		{
			return new CouponDto
			{
				CouponId = coupon.CouponId,
				CouponCode = coupon.CouponCode,
				MinAmount = coupon.MinAmount,
				DiscountAmount = coupon.DiscountAmount,
			};
		}

		public static Coupon ToCouponFromCreateDto(this CreateCouponDto createCouponDto)
		{
			return new Coupon
			{
				CouponCode = createCouponDto.CouponCode,
				MinAmount = createCouponDto.MinAmount,
				DiscountAmount = createCouponDto.DiscountAmount,
			};
		}

		public static Coupon ToCouponFromUpdateDto(this UpdateCouponRequestDto updateCouponDto)
		{
			return new Coupon
			{
				CouponCode = updateCouponDto.CouponCode,
				MinAmount = updateCouponDto.MinAmount,
				DiscountAmount = updateCouponDto.DiscountAmount,
			};
		}

		
	}
}
