using EMStores.Services.ProductAPI.Helpers;
using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;

namespace EMStores.Web.Services.IServices
{
	public interface IProductService
	{
		Task<ResponseDto?> CreateProductAsync(CreateProductDto createProductDto);
		Task<ResponseDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
		Task<ResponseDto?> DeleteProductAsync(int id);
		Task<ResponseDto?> GetProductByIdAsync(int id);
		Task<ResponseDto?> GetProductsAsync(ProductQuery query);
	}
}
