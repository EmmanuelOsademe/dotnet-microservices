using EMStores.Web.Models.Dtos.Coupon;

namespace EMStores.Web.Mapper
{
    public static class CouponMapper
	{
		public static Dictionary<string, string?> ConvertQueryObjectToDictionary(this CouponQuery query)
		{
			Dictionary<string, string?> queryDict = new();
			if(query.CouponCode != null)
			{
				queryDict.Add("CouponCode", query.CouponCode.ToString());
			}

			if(query.SortBy != null)
			{
				queryDict.Add("SortBy", query.SortBy.ToString());
			}

			if (query.IsDescending)
			{
				queryDict.Add("IsAscending", query.IsDescending.ToString());
			}

			if (query.PageNumber.ToString() == null)
			{
				queryDict.Add("PageNumber", query.PageNumber.ToString());
			}

			if (query.PageSize.ToString() == null)
			{
				queryDict.Add("PageSize", query.PageSize.ToString());
			}

			return queryDict; 
		}
	}
}
