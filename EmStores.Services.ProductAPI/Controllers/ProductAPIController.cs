using EmStores.Services.ProductAPI.Dtos;
using EmStores.Services.ProductAPI.Filters.ActionFilters;
using EmStores.Services.ProductAPI.Helpers;
using EmStores.Services.ProductAPI.Mappers;
using EmStores.Services.ProductAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmStores.Services.ProductAPI.Controllers
{
	[Route("api/products")]
	[ApiController]
	public class ProductAPIController(IProductRepository productRepository) : ControllerBase
	{
		private readonly IProductRepository _productRepo = productRepository;
		private readonly ResponseDto response = new ();

		[HttpPost]
		[ValidateCreateProductFilter]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto createProductDto)
		{
			try
			{
				var product = await _productRepo.CreateProductAsync(createProductDto.ToProductFromCreateProductDto());

				response.IsSuccess = product != null;
				response.Message = product == null ? "Product not created" : "Product successfully created";
				response.Result = product ?? null;

				return Ok(response);

			}
			catch(Exception e)
			{
				response.IsSuccess = false;
				response.Message = e.Message ?? "Something went wrong";
				return BadRequest(response);
			}
		}

		[HttpPut]
		[Route("{id:int}")]
		[ValidateProductIdFilter]
		[ValidateupdateProductFilter]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> UpdateProductAsync([FromRoute] int id, [FromBody] UpdateProductDto updateProductDto)
		{
			try
			{
				var product = await _productRepo.UpdateProductAsync(id, updateProductDto);

				response.IsSuccess = product != null;
				response.Message = product == null ? "Product not updated" : "Product successfully updated";
				response.Result = product ?? null;
				return Ok(response);
			} 
			catch (Exception e)
			{
				response.IsSuccess = false;
				response.Message = e.Message ?? "Something went wrong";
				return BadRequest(response);
			}
		}

		[HttpDelete]
		[Route("{id:int}")]
		[ValidateProductIdFilter]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> DeleteProductAsync([FromRoute] int id)
		{
			try
			{
				var product = await _productRepo.DeleteProductAsync(id);
				response.IsSuccess = product != null;
				response.Message = product == null ? "Product not found" : "Product successfully deleted";
				response.Result = product ?? null;
				return Ok(response);
			} 
			catch(Exception e)
			{
				response.IsSuccess = false;
				response.Message = e.Message ?? "Something went wrong";
				return BadRequest(response);
			}
		}

		[HttpGet]
		[Route("{id:int}")]
		[ValidateProductIdFilter]
		public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
		{
			try
			{
				var product = await _productRepo.GetProductByIdAsync(id);
				response.IsSuccess = product != null;
				response.Message = product == null ? "Product not found" : "Product successfully deleted";
				response.Result = product ?? null;
				return Ok(response);
			}
			catch(Exception e)
			{
				response.IsSuccess = false;
				response.Message = e.Message ?? "Something went wrong";
				return BadRequest(response);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetProductsAsync([FromQuery] ProductQuery query)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var products = await _productRepo.GetProductsAsync(query);
				response.IsSuccess = true;
				response.Result = products.Select(p => p.ToProductDto()).ToList();
				return Ok(response);
			} 
			catch(Exception e)
			{
				response.IsSuccess = false;
				response.Message = e.Message ?? "Something went wrong";
				return BadRequest(response);
			}
		}

	}
}
