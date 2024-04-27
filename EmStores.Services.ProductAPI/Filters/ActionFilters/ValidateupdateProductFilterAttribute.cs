﻿using EmStores.Services.ProductAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmStores.Services.ProductAPI.Filters.ActionFilters
{
	public class ValidateupdateProductFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			var productDto = context.ActionArguments["updateProductDto"] as UpdateProductDto;

			if(productDto == null)
			{
				context.ModelState.AddModelError("Product", "Product cannot be null");
				var problemDetails = new ValidationProblemDetails(context.ModelState)
				{
					Status = StatusCodes.Status400BadRequest
				};
				context.Result = new BadRequestObjectResult(problemDetails);
			}
		}
	}
}
