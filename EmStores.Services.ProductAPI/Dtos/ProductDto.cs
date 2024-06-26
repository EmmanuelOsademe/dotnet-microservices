﻿

namespace EmStores.Services.ProductAPI.Dtos
{
	public class ProductDto
	{
		public int ProductId { get; set; }

		public string Name { get; set; } = string.Empty;

		public double Price { get; set; }
		public string Description { get; set; } = string.Empty;

		
		public string Category { get; set; } = string.Empty;

		public string ImageUrl { get; set; } = string.Empty;
		public string? ImageLocalPath { get; set; } = string.Empty;
		public IFormFile? Image { get; set; }
	}
}
