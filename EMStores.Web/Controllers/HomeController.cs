using EMStores.Services.ProductAPI.Helpers;
using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Models.Dtos.Cart;
using EMStores.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace EMStores.Web.Controllers
{
	public class HomeController(IProductService productService, ICartService cartService) : Controller
	{

		private readonly IProductService _productService = productService;
        private readonly ICartService _cartService = cartService;


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

        [HttpPost]
        [Authorize]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value ?? string.Empty;
            CartHeaderInputDto header = new() { UserId = userId};
            CartDetailsInputDto details = new() { ProductId = productDto.ProductId, Count = productDto.Count }; 

            CartInputDto inputDto = new() { CartHeaderInputDto = header, CartDetailsInputDto = details };

            ResponseDto? response = await _cartService.UpsertCartAsync(inputDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item has be added to Shopping Cart";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["error"] = "Error adding item to Shopping Cart";
                return View(productDto);
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
