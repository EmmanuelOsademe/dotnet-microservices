using EMStores.Services.ProductAPI.Helpers;
using EMStores.Web.Models;
using EMStores.Web.Mapper;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Services.IServices;
using EMStores.Web.Utility;

namespace EMStores.Web.Services
{
	public class ProductService(IBaseService baseService) : IProductService
	{
		private readonly IBaseService _baseService = baseService;
		public async Task<ResponseDto?> CreateProductAsync(CreateProductDto createProductDto)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.POST,
				ApiUrl = StaticDetails.ProductAPIBaseUrl + "/api/product",
				Data = createProductDto,
				ContentType = StaticDetails.ContentType.MultipartFormData
			};

			return await _baseService.SendAsync(requestDto);
		}

		public async Task<ResponseDto?> DeleteProductAsync(int id)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.DELETE,
				ApiUrl = StaticDetails.ProductAPIBaseUrl + $"/api/product/{id}"
			};

			return await _baseService.SendAsync(requestDto);
		}

		public async Task<ResponseDto?> GetProductByIdAsync(int id)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.GET,
				ApiUrl = StaticDetails.ProductAPIBaseUrl + $"/api/product/{id}"
			};

			return await _baseService.SendAsync(requestDto);
		}

		public async Task<ResponseDto?> GetProductsAsync(ProductQuery query)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.GET,
				ApiUrl = StaticDetails.ProductAPIBaseUrl + "/api/product",
				Query = query.ConvertQueryToDictionary()
			};

			return await _baseService.SendAsync(requestDto);
		}

		public async Task<ResponseDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
		{
			RequestDto requestDto = new()
			{
				ApiType = StaticDetails.ApiType.PUT,
				ApiUrl = StaticDetails.ProductAPIBaseUrl + $"/api/product/{id}",
				Data = updateProductDto,
				ContentType = StaticDetails.ContentType.MultipartFormData
			};

			return await _baseService.SendAsync(requestDto);
		}
	}
}
