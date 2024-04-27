using static EMStores.Web.Utility.StaticDetails;

namespace EMStores.Web.Models.Dtos
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string ApiUrl { get; set; } = string.Empty;
        public object? Data { get; set; }

        public string AccessToken = string.Empty;

        public Dictionary<string, string?>? Query { get; set; }
    }
}
