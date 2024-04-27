namespace EMStore.Services.CouponAPI.Helpers
{
	public class CouponQuery
	{
		public string? CouponCode { get; set; } = string.Empty;
		public string? SortBy { get; set; } = string.Empty;
		public bool IsDescending { get; set; } = false;
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 5;
	}
}
