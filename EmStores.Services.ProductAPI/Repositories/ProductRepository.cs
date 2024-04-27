using EmStores.Services.ProductAPI.Data;
using EmStores.Services.ProductAPI.Dtos;
using EmStores.Services.ProductAPI.Helpers;
using EmStores.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmStores.Services.ProductAPI.Repositories
{
	public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
	{
		private readonly ApplicationDbContext _dbContext = dbContext;
		public async Task<Product?> CreateProductAsync(Product product)
		{
			var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == product.Name.ToLower() && p.Category.ToLower() == product.Category.ToLower());
			if (existingProduct != null)
			{
				return null;
			}

			await _dbContext.Products.AddAsync(product);
			await _dbContext.SaveChangesAsync();
			return product;
		}

		public async Task<Product?> DeleteProductAsync(int id)
		{
			var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);
			if (product != null)
			{
				_dbContext.Products.Remove(product);
				await _dbContext.SaveChangesAsync();
			}
			return product ?? null;
		}

		public async Task<Product?> GetProductByIdAsync(int id)
		{
			var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);
			return product ?? null;
		}

		public async Task<List<Product>> GetProductsAsync(ProductQuery query)
		{
			var products = _dbContext.Products.AsQueryable();

			if (!string.IsNullOrWhiteSpace(query.Description))
			{
				products.Where(p => p.Description.ToLower().Contains(query.Description.ToLower()));
			}

			if (!string.IsNullOrWhiteSpace(query.Name))
			{
				products.Where(p => p.Name == query.Name);
			}

			if (!string.IsNullOrWhiteSpace(query.Category))
			{
				products.Where(p => p.Category == query.Category);
			}

			if (!string.IsNullOrWhiteSpace(query.SortBy))
			{
				if(query.SortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
				{
					products = query.IsDescending ? products.OrderByDescending(p => p.Price) : products.OrderBy(p => p.Price);
				}
			}

			int skipAmount = (query.PageNumber - 1) * query.PageSize;	

			products = products.Skip(skipAmount).Take(query.PageSize);


			return await products.ToListAsync();
		}

		public async Task<Product?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
		{
			var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);
			if(product == null)
			{
				return null;
			}

			var similarProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == updateProductDto.Name.ToLower() && p.Category.ToLower() == updateProductDto.Category.ToLower());
			if(similarProduct != null && similarProduct.ProductId != product.ProductId)
			{
				return null;
			}

			product.Name = updateProductDto.Name;
			product.Price = updateProductDto.Price;
			product.Description = updateProductDto.Description;
			product.Category = updateProductDto.Category;
			product.ImageUrl = updateProductDto.ImageUrl;

			await _dbContext.SaveChangesAsync();
			return product;
		}
	}
}
