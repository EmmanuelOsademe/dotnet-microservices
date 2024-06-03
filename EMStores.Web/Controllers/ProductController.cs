using EMStores.Services.ProductAPI.Helpers;
using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EMStores.Web.Controllers
{
	public class ProductController(IProductService productService) : Controller
	{
		private readonly IProductService _productService = productService;

		[HttpGet]
		public async Task<IActionResult> ProductIndex()
		{
			List<ProductDto>? products = [];
			ProductQuery query = new ();
			ResponseDto? response = await _productService.GetProductsAsync(query);
			if(response != null && response.IsSuccess)
			{
				products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result)); ;

			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(products);
		}

		[HttpGet]
		public async Task<IActionResult> ProductCreate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ProductCreate(CreateProductDto createProductDto)
		{
			
			ResponseDto? response = await _productService.CreateProductAsync(createProductDto);
			if(response != null && response.IsSuccess)
			{
				TempData["success"] = "Product created successfully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message ?? "Error creating product";
			}
			
			return View(createProductDto);
		}

		[HttpGet]
		public async Task<IActionResult> ProductDelete(int productId)
		{
			ResponseDto? response = await _productService.GetProductByIdAsync(productId);
			if(response != null && response.IsSuccess)
			{
				ProductDto product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result)) ?? new ProductDto();
				return View(product);

			}
			else
			{
				return NotFound();
			}
		}

		[HttpPost]
		public async Task<IActionResult> ProductDelete(ProductDto productDto)
		{
			ResponseDto? response = await _productService.DeleteProductAsync(productDto.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product Deleted Successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }

        [HttpGet]
        public async Task<IActionResult> ProductUpdate(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result)) ?? new ProductDto();
                return View(product);

            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductDto productDto)
        {
			UpdateProductDto updateProductDto = new()
			{
				Name = productDto.Name,
				Category = productDto.Category,
				Price = productDto.Price,
				Description = productDto.Description,
				ImageUrl = productDto.ImageUrl,
				Image = productDto.Image,
				ImageLocalPath = productDto.ImageLocalPath
			};


			ResponseDto? response = await _productService.UpdateProductAsync(productDto.ProductId, updateProductDto);
			if(response != null && response.IsSuccess)
			{
				TempData["success"] = "Product Updated Successfully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message ?? "Error Updating Product";
			}
			return View(productDto);
        }
    }
}
