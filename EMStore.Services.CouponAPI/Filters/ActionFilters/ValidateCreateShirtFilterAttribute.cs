using EMStore.Services.CouponAPI.Data;
using EMStore.Services.CouponAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EMStore.Services.CouponAPI.Filters.ActionFilters
{
	public class ValidateCreateShirtFilterAttribute : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
			var coupon = context.ActionArguments["couponDto"] as CreateCouponDto;

			if (coupon == null)
			{
				context.ModelState.AddModelError("Coupon", "Coupon object cannot be null");
				var problemDetails = new ValidationProblemDetails(context.ModelState) { Status = StatusCodes.Status400BadRequest };
				context.Result = new BadRequestObjectResult(problemDetails);
			}
		}
	}
}
