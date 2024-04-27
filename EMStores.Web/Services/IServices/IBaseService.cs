using EMStores.Web.Models.Dtos;

namespace EMStores.Web.Services.IServices
{
    public interface IBaseService
	{
		Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
	}
}
