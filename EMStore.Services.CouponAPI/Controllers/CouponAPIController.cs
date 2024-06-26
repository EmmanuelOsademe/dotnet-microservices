﻿using EMStore.Services.CouponAPI.Dtos;
using EMStore.Services.CouponAPI.Filters.ActionFilters;
using EMStore.Services.CouponAPI.Helpers;
using EMStore.Services.CouponAPI.Mappers;
using EMStore.Services.CouponAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace EMStore.Services.CouponAPI.Controllers
{
	[Route("api/coupon")]
    [ApiController]
	[Authorize]
	public class CouponAPIController(ICouponRepository couponRepo) : ControllerBase
	{

		private readonly ICouponRepository _couponRepo = couponRepo;
        private ResponseDto response = new();

		[HttpGet]
		public async Task<IActionResult> GetAllAsync([FromQuery] CouponQuery query)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var coupons = await _couponRepo.GetAllAsync(query);
				response.IsSuccess = true;
				response.Result = coupons.Select(c => c.ToCouponDto()).ToList();
				return Ok(response);
			}
			catch(Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message ?? "An error occurred";
				return BadRequest(response);	
			}
		}

		[HttpGet]
		[Route("{id:int}")]
		[ValidateCouponIdFilter]
		public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
		{
			try
			{
				var coupon = await _couponRepo.GetByIdAsync(id);
				if(coupon == null)
				{
					response.IsSuccess = false;
					response.Message = "Coupon not found";
				}
				else
				{
					response.IsSuccess = true;
					response.Result = coupon.ToCouponDto();
				}
				
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message ?? "Something went wrong";
				return BadRequest(response);
			}
			
		}

		[HttpGet]
		[Route("GetByCode/{couponCode}")]
		[ValidateCouponCodeFilter]
		public async Task<IActionResult> GetByCouponCodeAsync([FromRoute] string couponCode)
		{
			try
			{
				var coupon = await _couponRepo.GetByCouponCodeAsync(couponCode);
				if (coupon == null)
				{
					response.IsSuccess = false;
					response.Message = "Coupon not found";
				}
				else
				{
					response.IsSuccess = true;
					response.Result = coupon.ToCouponDto();
				}

				return Ok(response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message ?? "Something went wrong";
				return BadRequest(response);
			}

		}

		[HttpPost]
		[ValidateCreateShirtFilter]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> CreateCouponAsync([FromBody] CreateCouponDto couponDto)
		{
			try
			{
				var coupon = await _couponRepo.CreateCouponAsync(couponDto.ToCouponFromCreateDto());
				if (coupon == null)
				{
                    response.IsSuccess = false;
					response.Message = "Coupon not created";
				}
				else
				{
                    // Create the coupon in Stripe
                    var options = new CouponCreateOptions
                    {
                        Name = couponDto.CouponCode,
                        Currency = "usd",
                        Id = couponDto.CouponCode,
                        AmountOff = (long)(couponDto.DiscountAmount * 100),
                    };
                    var service = new CouponService();
                    service.Create(options);

                    response.IsSuccess = true;
					response.Result = coupon.ToCouponDto();
				}
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message ?? "Something went wrong";
				return BadRequest(response);
			}
		}

		[HttpPut]
		[Route("{id:int}")]
		[ValidateCouponIdFilter]
		[ValidateUpdateShirtFilter]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> UpdateCouponAsync([FromRoute] int id, [FromBody] UpdateCouponRequestDto updateCouponDto)
		{
			try
			{
				var coupon = await _couponRepo.UpdateCouponAsync(id, updateCouponDto.ToCouponFromUpdateDto());
				if (coupon == null)
				{
					response.IsSuccess = false;
					response.Message = "Coupon not found";
				}
				else
				{
                    response.IsSuccess = true;
					response.Result = coupon.ToCouponDto();
				}

				return Ok(response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message ?? "Something went wrong";
				return BadRequest(response);
			}
		}

		[HttpDelete]
		[Route("{id:int}")]
		[ValidateCouponIdFilter]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id)
		{
			try
			{
				var coupon = await _couponRepo.DeleteCouponByIdAsync(id);
				if (coupon == null)
				{
                    response.IsSuccess = false;
					response.Message = "Coupon not found";
				}
				else
				{
                    // Delete the coupon from stripe
                    var service = new CouponService();
                    service.Delete(coupon.CouponCode);

                    response.IsSuccess = true;
					response.Result = coupon.ToCouponDto();
				}

				return Ok(response);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message ?? "Something went wrong";
				return BadRequest(response);
			}

		}
	}
}
