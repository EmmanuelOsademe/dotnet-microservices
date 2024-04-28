using EMStores.Services.ProductAPI.Helpers;
using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EMStores.Web.Controllers
{
	public class HomeController(IProductService productService) : Controller
	{

		private readonly IProductService _productService = productService;


		public async Task<IActionResult> Index()
		{
            List<ProductDto>? products = [];
            ProductQuery query = new();
            ResponseDto? response = await _productService.GetProductsAsync(query);
            if (response != null && response.IsSuccess)
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
        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
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

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
