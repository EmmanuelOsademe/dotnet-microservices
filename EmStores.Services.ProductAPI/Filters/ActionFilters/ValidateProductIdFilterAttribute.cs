using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmStores.Services.ProductAPI.Filters.ActionFilters
{
	public class ValidateProductIdFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			var productId = context.ActionArguments["id"] as int?;

			if(!productId.HasValue || productId.Value < 1)
			{
				context.ModelState.AddModelError("ProductId", "Invalid product Id provided");

				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status400BadRequest
				};

				context.Result = new BadRequestObjectResult(problemDetails);
			}
		}
	}
}
