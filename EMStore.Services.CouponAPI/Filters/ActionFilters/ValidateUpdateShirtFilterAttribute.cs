using EMStore.Services.CouponAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EMStore.Services.CouponAPI.Filters.ActionFilters
{
	public class ValidateUpdateShirtFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
			var coupon = context.ActionArguments["updateCouponDto"] as UpdateCouponRequestDto;

			if (coupon == null)
			{
				context.ModelState.AddModelError("Coupon", "Coupon object cannot be null");
				var problemDetails = new ValidationProblemDetails(context.ModelState) { Status = StatusCodes.Status400BadRequest };
				context.Result = new BadRequestObjectResult(problemDetails);
			}
		}
	}
}
