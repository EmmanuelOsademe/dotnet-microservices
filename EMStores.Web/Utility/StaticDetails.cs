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

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_ReadyForPickup = "ReadyForPickup";
        public const string Status_Completed = "Completed";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";

        public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}
	}
}
