using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EMStore.Services.CouponAPI.Filters.ActionFilters
{
	public class ValidateCouponIdFilterAttribute : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			var couponId = context.ActionArguments["id"] as int?;

			if(!couponId.HasValue || couponId.Value < 1)
			{
				context.ModelState.AddModelError("CouponId", "CouponId is invalid");

				var problemDetails = new ValidationProblemDetails(context.ModelState) { Status = StatusCodes.Status400BadRequest };
				context.Result = new BadRequestObjectResult(problemDetails);
			}
		}
	}
}
