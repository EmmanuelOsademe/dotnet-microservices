using EMStores.Services.ProductAPI.Helpers;

namespace EMStores.Web.Mapper
{
	public static class Product
	{

		public static Dictionary<string, string?> ConvertQueryToDictionary(this ProductQuery query)
		{
			Dictionary<string, string?> queryDict = new();

			if(query.Name != null)
			{
				queryDict["Name"] = query.Name;
			}

			if(query.Description != null)
			{
				queryDict["Description"] = query.Description;
			}

			if (query.SortBy != null)
			{
				queryDict["SortBy"] = query.SortBy;
			}

			if (query.IsDescending)
			{
				queryDict["IsDescending"] = query.IsDescending.ToString();
			}

			if(query.PageNumber.ToString() != null)
			{
				queryDict["PageNumber"] = query.PageNumber.ToString();
			}

			if(query.PageSize.ToString() != null)
			{
				queryDict["PageSize"] = query.PageSize.ToString();
			}

			return queryDict;
		}
	}
}
