using System.ComponentModel.DataAnnotations;

namespace EMStores.Web.Models
{
	public class UpdateProductDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		[Range(1, 1000)]
		public double Price { get; set; }

		[Required]
		public string Description { get; set; } = string.Empty;

		[Required]
		public string Category { get; set; } = string.Empty;

		[Required]
		public string ImageUrl { get; set; } = string.Empty;
	}
}
