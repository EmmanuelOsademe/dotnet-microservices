using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EMStore.Services.CouponAPI.Filters.ActionFilters
{
	public class ValidateCouponCodeFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			var couponCode = context.ActionArguments["couponCode"] as string;

			if (string.IsNullOrEmpty(couponCode))
			{
				context.ModelState.AddModelError("CouponCode", "Coupon code is invalid");

				var problemDetails = new ValidationProblemDetails(context.ModelState) { Status = StatusCodes.Status400BadRequest };
				context.Result = new BadRequestObjectResult(problemDetails);
			}
		}
	}
}
