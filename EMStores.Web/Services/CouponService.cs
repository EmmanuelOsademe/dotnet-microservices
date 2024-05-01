using EMStores.Web.Mapper;
using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Models.Dtos.Coupon;
using EMStores.Web.Services.IServices;
using EMStores.Web.Utility;

namespace EMStores.Web.Services
{
    public class CouponService(IBaseService baseService) : ICouponService
	{
		private readonly IBaseService _baseService = baseService;
		public async Task<ResponseDto?> CreateCouponAsync(CreateCouponDto couponDto)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.POST,
				ApiUrl = StaticDetails.CouponAPIBaseUrl+"/api/coupon",
				Data = couponDto
			};

			return await _baseService.SendAsync(requestDto);
		}

		public async Task<ResponseDto?> DeleteCouponByIdAsync(int id)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.DELETE,
				ApiUrl = StaticDetails.CouponAPIBaseUrl + $"/api/coupon/{id}"
			};

			return await _baseService.SendAsync(requestDto);
		}

		public async Task<ResponseDto?> GetAllAsync(CouponQuery query)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.GET,
				ApiUrl = StaticDetails.CouponAPIBaseUrl + "/api/coupon",
				Query = query.ConvertQueryObjectToDictionary()
			};

			return await _baseService.SendAsync(requestDto);
		}

		public async Task<ResponseDto?> GetByCouponCodeAsync(string couponCode)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.GET,
				ApiUrl = StaticDetails.CouponAPIBaseUrl + $"/api/coupon/GetByCode/{couponCode}"
			};

			return await _baseService.SendAsync(requestDto);
		}

		public async Task<ResponseDto?> GetByIdAsync(int id)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.GET,
				ApiUrl = StaticDetails.CouponAPIBaseUrl + $"/api/coupon/{id}"
			};

			return await _baseService.SendAsync(requestDto);
		}

		public async Task<ResponseDto?> UpdateCouponAsync(int id, UpdateCouponDto updateCouponDto)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.PUT,
				ApiUrl = StaticDetails.CouponAPIBaseUrl + $"/api/coupon/{id}",
				Data = updateCouponDto
			};

			return await _baseService.SendAsync(requestDto);
		}
	}
}
