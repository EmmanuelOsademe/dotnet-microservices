using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Models.Dtos.Coupon;

namespace EMStores.Web.Services.IServices
{
    public interface ICouponService
	{
		Task<ResponseDto?>  CreateCouponAsync(CreateCouponDto couponDto);
		Task<ResponseDto?> DeleteCouponByIdAsync(int id);
		Task<ResponseDto?> GetAllAsync(CouponQuery query);
		Task<ResponseDto?> GetByCouponCodeAsync(string couponCode);
		Task<ResponseDto?> GetByIdAsync(int id);
		Task<ResponseDto?> UpdateCouponAsync(int id, UpdateCouponDto updateCouponDto);
	}
}
