using EmStores.Services.ProductAPI.Dtos;
using EmStores.Services.ProductAPI.Helpers;
using EmStores.Services.ProductAPI.Models;

namespace EmStores.Services.ProductAPI.Repositories
{
	public interface IProductRepository
	{
		Task<Product?> CreateProductAsync(Product product);
		Task<Product?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);	
		Task<Product?> GetProductByIdAsync(int id);
		Task<Product?> DeleteProductAsync(int id);
		Task<List<Product>> GetProductsAsync(ProductQuery query);
	}
}
