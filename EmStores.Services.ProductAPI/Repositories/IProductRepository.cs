using EmStores.Services.ProductAPI.Dtos;
using EmStores.Services.ProductAPI.Helpers;
using EmStores.Services.ProductAPI.Models;

namespace EmStores.Services.ProductAPI.Repositories
{
	public interface IProductRepository
	{
		Task<ProductDto?> CreateProductAsync(CreateProductDto product, string baseUrl);
		Task<Product?> UpdateProductAsync(int id, UpdateProductDto updateProductDto, string baseUrl);	
		Task<Product?> GetProductByIdAsync(int id);
		Task<Product?> DeleteProductAsync(int id);
		Task<List<Product>> GetProductsAsync(ProductQuery query);
	}
}
