using EMStore.Services.CouponAPI.Data;
using EMStore.Services.CouponAPI.Dtos;
using EMStore.Services.CouponAPI.Helpers;
using EMStore.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.CouponAPI.Repositories
{
	public class CouponRepository(ApplicationDbContext dbContext) : ICouponRepository
	{
		private readonly ApplicationDbContext _dbContext = dbContext;

		public async Task<List<Coupon>> GetAllAsync(CouponQuery query)
		{
			var coupons = _dbContext.Coupons.AsQueryable();

			if (!string.IsNullOrWhiteSpace(query.CouponCode))
			{
				coupons = coupons.Where(c => c.CouponCode.Contains(query.CouponCode));
			}

			if (!string.IsNullOrWhiteSpace(query.SortBy))
			{
				if (query.SortBy.Equals("DiscountAmount", StringComparison.OrdinalIgnoreCase))
				{
					coupons = query.IsDescending ? coupons.OrderByDescending(c => c.DiscountAmount) : coupons.OrderBy(c => c.DiscountAmount);
				}

				if (query.SortBy.Equals("MinAmount", StringComparison.OrdinalIgnoreCase))
				{
					coupons = query.IsDescending ? coupons.OrderByDescending(c => c.MinAmount) : coupons.OrderBy(c => c.MinAmount);
				}
			}

			int skipAmount = (query.PageNumber - 1) * query.PageSize;

			coupons = coupons.Skip(skipAmount).Take(query.PageSize);

			return await coupons.ToListAsync();
		}

		public async Task<Coupon?> GetByIdAsync(int id)
		{
			var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.CouponId == id);
			if(coupon == null)
			{
				return null;
			}
			return coupon;
		}

		public async Task<Coupon?> GetByCouponCodeAsync(string couponCode)
		{
			var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.CouponCode.ToLower() == couponCode.ToLower());
			if (coupon == null) return null;
			return coupon;
		}

		public async Task<Coupon?> CreateCouponAsync(Coupon coupon)
		{
			var exisitingCoupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => !string.IsNullOrWhiteSpace(coupon.CouponCode) && coupon.CouponCode.ToLower() == c.CouponCode.ToLower() && coupon.MinAmount == c.MinAmount && coupon.DiscountAmount == c.DiscountAmount);
			if(exisitingCoupon != null)
			{
				return null;
			}
			await _dbContext.Coupons.AddAsync(coupon);
			await _dbContext.SaveChangesAsync();
			return coupon;
		}

		public async Task<Coupon?> UpdateCouponAsync(int id, Coupon updateCouponDto)
		{
			var existingCoupon = await GetByIdAsync(id);
			if(existingCoupon == null)
			{
				return null;
			}

			existingCoupon.CouponCode = updateCouponDto.CouponCode;
			existingCoupon.MinAmount = updateCouponDto.MinAmount;
			existingCoupon.DiscountAmount = updateCouponDto.DiscountAmount;

			await _dbContext.SaveChangesAsync();
			return existingCoupon;

		}

		public async Task<Coupon?> DeleteCouponByIdAsync(int id)
		{
			var coupon = await GetByIdAsync(id);

			if(coupon == null)
			{
				return null;
			}

			_dbContext.Coupons.Remove(coupon);
			await _dbContext.SaveChangesAsync();
			return coupon;
		}
	}
}
