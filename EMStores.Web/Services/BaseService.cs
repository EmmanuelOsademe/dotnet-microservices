using EMStores.Web.Models.Dtos;
using EMStores.Web.Services.IServices;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static EMStores.Web.Utility.StaticDetails;

namespace EMStores.Web.Services
{
    public class BaseService(IHttpClientFactory clientFactory, ITokenProvider tokenProvider) : IBaseService
	{
		private readonly IHttpClientFactory _clientFactory = clientFactory;
		private readonly ITokenProvider _tokenProvider = tokenProvider;
		public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer)
		{
			HttpClient client = _clientFactory.CreateClient("EMStoresApi");
			HttpRequestMessage message = new();

			if(requestDto.ContentType == ContentType.MultipartFormData)
			{
				message.Headers.Add("Accept", "*/*");
			}
			else
			{
				message.Headers.Add("Accept", "application/json");
			}

			// token
			if (withBearer)
			{
				var token = _tokenProvider.GetToken();
				message.Headers.Add("Authorization", $"Bearer {token}");
			}

			message.RequestUri = new Uri(requestDto.ApiUrl);

			if(requestDto.ContentType == ContentType.MultipartFormData)
			{
				var content = new MultipartFormDataContent();
				foreach(var prop in requestDto.Data.GetType().GetProperties())
				{
					var value = prop.GetValue(requestDto.Data);
					if(value is FormFile)
					{
						var file = (FormFile)value;
						if(file != null)
						{
							content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
						}
					}
				}
			}
			else
			{
				if (requestDto.Data != null)
				{
					// Use newton-soft for serialization
					message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
				}
			}

			if(requestDto.Query != null)
			{
				message.RequestUri = new Uri(QueryHelpers.AddQueryString(requestDto.ApiUrl, requestDto.Query));
			}

			switch (requestDto.ApiType)
			{
				case ApiType.POST:
					message.Method = HttpMethod.Post;
					break;
				case ApiType.PUT:
					message.Method = HttpMethod.Put;
					break;
				case ApiType.DELETE:
					message.Method = HttpMethod.Delete;
					break;
				default:
					message.Method = HttpMethod.Get;
					break;
			}

			try
			{
				HttpResponseMessage apiResponse = await client.SendAsync(message);
				switch (apiResponse.StatusCode)
				{
					case HttpStatusCode.NotFound:
						return new ResponseDto() {IsSuccess = false, Message = "Not Found" };
					case HttpStatusCode.Forbidden:
						return new ResponseDto() { IsSuccess = false, Message = "Access Denied" };
					case HttpStatusCode.Unauthorized:
						return new ResponseDto() { IsSuccess = false, Message = "Unauthorized" };
					case HttpStatusCode.InternalServerError:
						return new ResponseDto() { IsSuccess = false, Message = "Interval Server Error" };
					case HttpStatusCode.BadRequest:
						return new ResponseDto() { IsSuccess = false, Message = "Bad Request" };
					default:
						var apiContent = await apiResponse.Content.ReadAsStringAsync();
						var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
						return apiResponseDto;
				}
			}
			catch(Exception ex)
			{
				return new ResponseDto() { IsSuccess = false, Message = ex.Message.ToString()??"Something went wrong" };
			}
		}
	}
}
