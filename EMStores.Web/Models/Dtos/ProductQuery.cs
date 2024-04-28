namespace EMStores.Services.ProductAPI.Helpers
{
	public class ProductQuery
	{
		public string? Name { get; set; } = string.Empty;
		public string? Description { get; set; } = string.Empty;
		public string? Category { get; set; }  = string.Empty;
		public string? SortBy { get; set;} = string.Empty;
		public bool IsDescending { get; set; } = false;

		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 5;
	}
}
