using EMStore.Services.CouponAPI.Dtos;
using EMStore.Services.CouponAPI.Helpers;
using EMStore.Services.CouponAPI.Models;

namespace EMStore.Services.CouponAPI.Repositories
{
	public interface ICouponRepository
	{
		Task<Coupon?> CreateCouponAsync(Coupon coupon);
		Task<Coupon?> DeleteCouponByIdAsync(int id);
		Task<List<Coupon>> GetAllAsync(CouponQuery query);
		Task<Coupon?> GetByCouponCodeAsync(string couponCode);
		Task<Coupon?> GetByIdAsync(int id);
		Task<Coupon?> UpdateCouponAsync(int id, Coupon updateCouponDto);
	}
}