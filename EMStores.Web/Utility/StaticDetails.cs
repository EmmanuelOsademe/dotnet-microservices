namespace EMStores.Web.Utility
{
	public class StaticDetails
	{
		public static string CouponAPIBaseUrl { get; set; } = string.Empty;
		public static string AuthAPIBaseUrl { get; set;} = string.Empty;
		public static string ProductAPIBaseUrl { get; set; } = string.Empty;
		public static string ShoppingCartAPIBaseUrl { get; set; } = string.Empty;
        public static string OrderAPIBaseUrl { get; set; } = string.Empty;

        public const string RoleAdmin = "ADMIN";
		public const string RoleCustomer = "CUSTOMER";
		public const string TokenCookie = "JwtToken";

		public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}
	}
}
