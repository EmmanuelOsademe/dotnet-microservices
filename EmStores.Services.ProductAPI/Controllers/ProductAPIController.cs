using EmStores.Services.ProductAPI.Dtos;
using EmStores.Services.ProductAPI.Filters.ActionFilters;
using EmStores.Services.ProductAPI.Helpers;
using EmStores.Services.ProductAPI.Mappers;
using EmStores.Services.ProductAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmStores.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using EmStores.Services.ProductAPI.Models;

namespace EmStores.Services.ProductAPI.Controllers
{
	[Route("api/product")]
	[ApiController]
	public class ProductAPIController(IProductRepository productRepository, ApplicationDbContext dbContext) : ControllerBase
	{
		private readonly IProductRepository _productRepo = productRepository;
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ResponseDto response = new ();

		[HttpPost]
		//[ValidateCreateProductFilter]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> CreateProductAsync(CreateProductDto createProductDto)
		{
			try
			{

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
				var product = await _productRepo.CreateProductAsync(createProductDto, baseUrl);

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
		public async Task<IActionResult> UpdateProductAsync([FromRoute] int id, UpdateProductDto updateProductDto)
		{
			try
			{
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                var product = await _productRepo.UpdateProductAsync(id, updateProductDto, baseUrl);

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
