using EmStores.Services.ProductAPI.Dtos;
using EmStores.Services.ProductAPI.Models;

namespace EmStores.Services.ProductAPI.Mappers
{
	public static class ProductMapper
	{
		public static ProductDto ToProductDto(this Product product)
		{
			return new ProductDto
			{
				ProductId = product.ProductId,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				Category = product.Category,
				ImageUrl = product.ImageUrl,
				ImageLocalPath  = product.ImageLocalPath,
			};
		}

		public static Product ToProductFromCreateProductDto (this CreateProductDto createProductDto)
		{
			return new Product
			{
				Name = createProductDto.Name,
				Description = createProductDto.Description,
				Price = createProductDto.Price,
				Category = createProductDto.Category,
				ImageUrl = createProductDto.ImageUrl
			};
		}
	}
}
