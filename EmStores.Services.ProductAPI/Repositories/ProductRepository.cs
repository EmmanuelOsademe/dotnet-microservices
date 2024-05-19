using EmStores.Services.ProductAPI.Data;
using EmStores.Services.ProductAPI.Dtos;
using EmStores.Services.ProductAPI.Helpers;
using EmStores.Services.ProductAPI.Mappers;
using EmStores.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmStores.Services.ProductAPI.Repositories
{
	public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
	{
		private readonly ApplicationDbContext _dbContext = dbContext;
		public async Task<ProductDto?> CreateProductAsync(CreateProductDto productDto, string baseUrl)
		{
			var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == productDto.Name.ToLower() && p.Category.ToLower() == productDto.Category.ToLower());
			if (existingProduct != null)
			{
				return null;
			}

			Product product = productDto.ToProductFromCreateProductDto();
			_dbContext.Products.Add(product);
			_dbContext.SaveChanges();

			if(productDto.Image != null)
			{
				string fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
				string filePath = @"wwwroot\ProductImages\" + fileName;
				var filePathDir = Path.Combine(Directory.GetCurrentDirectory(), filePath);
				using (var filestream = new FileStream(filePathDir, FileMode.Create))
				{
					productDto.Image.CopyTo(filestream);
				}

				product.ImageUrl = $"{baseUrl}/ProductImages/{filePath}";
				product.ImageLocalPath = filePath;
			}
			else
			{
				product.ImageUrl = "https://placehold.co/600x400";
			}

			_dbContext.Products.Update(product);
			_dbContext.SaveChanges();

			return product.ToProductDto();
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
