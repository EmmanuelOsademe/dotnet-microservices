using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Models.Dtos.Coupon;
using EMStores.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EMStores.Web.Controllers
{
    public class CouponController(ICouponService couponService) : Controller
    {
        private readonly ICouponService _couponService = couponService;

        [HttpGet]
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? coupons = [];

            CouponQuery query = new();

            ResponseDto? response = await _couponService.GetAllAsync(query);

            if(response != null && response.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(coupons);
        }


        [HttpGet]
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CreateCouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(couponDto);

                if(response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon Created Successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(couponDto);
        }

        [HttpGet]
        public async Task<IActionResult> CouponDelete(int couponId)
        {

            ResponseDto? response = await _couponService.GetByIdAsync(couponId);

            if(response != null && response.IsSuccess)
            {
                CouponDto coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result)) ?? new CouponDto();

                return View(coupon);
            }
            else
            {
                return NotFound();
            }
   
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {

            ResponseDto? response = await _couponService.DeleteCouponByIdAsync(couponDto.CouponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Deleted Successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(couponDto);
        }

        [HttpGet]
        public async Task<IActionResult> CouponUpdate(int couponId)
        {

            ResponseDto? response = await _couponService.GetByIdAsync(couponId);

            if (response != null && response.IsSuccess)
            {
                CouponDto coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result)) ?? new CouponDto();
                return View(coupon);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
            

        }

		[HttpPost]
		public async Task<IActionResult> CouponUpdate(CouponDto couponDto)
		{
            UpdateCouponDto updateDto = new() { CouponCode = couponDto.CouponCode, MinAmount = couponDto.MinAmount, DiscountAmount = couponDto.DiscountAmount };
			ResponseDto? response = await _couponService.UpdateCouponAsync(couponDto.CouponId, updateDto);

			if (response != null && response.IsSuccess)
			{
                TempData["success"] = "Coupon Updated Successfully";
                return RedirectToAction(nameof(CouponIndex));
			}
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(couponDto);
		}


	}
}
