using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Services.IServices;
using EMStores.Web.Utility;

namespace EMStores.Web.Services
{
    public class AuthService(IBaseService baseService) : IAuthService
    {
        private readonly IBaseService _baseService = baseService;
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            RequestDto requestDto = new()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.AuthAPIBaseUrl + "/api/auth/AssignRole",
                Data = registrationRequestDTO
            };

            return await _baseService.SendAsync(requestDto);
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            RequestDto requestDto = new()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.AuthAPIBaseUrl + "/api/auth/Login",
                Data = loginRequestDTO
            };

            return await _baseService.SendAsync(requestDto, false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            RequestDto requestDto = new()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.AuthAPIBaseUrl + "/api/auth/Register",
                Data = registrationRequestDTO
            };

            return await _baseService.SendAsync(requestDto, false);
        }
    }
}
