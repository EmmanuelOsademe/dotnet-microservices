using AutoMapper;
using EMStore.Services.CouponAPI.Dtos;
using EMStore.Services.CouponAPI.Models;

namespace EMStore.Services.CouponAPI.Mappers
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<CouponDto, Coupon>();
				config.CreateMap<Coupon, CouponDto>();
			});
			return mappingConfig;
		}
	}
}
