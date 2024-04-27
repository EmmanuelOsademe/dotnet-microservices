using EMStore.Services.AuthAPI.DTOs;
using EMStore.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EMStore.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        protected ResponseDTO responseDto = new();

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO requestDTO)
        {
            var errorMessage = await _authService.Register(requestDTO);
            if (!string.IsNullOrEmpty(errorMessage)){
                responseDto.IsSuccess = false;
                responseDto.Message = errorMessage;
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var loginResponse = await _authService.Login(loginRequest);
            if(loginResponse != null && loginResponse.User == null)
            {
                return BadRequest(loginResponse);
            }
            responseDto.Result = loginResponse;
            return Ok(responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO requestDTO)
        {
            var response = await _authService.AssignRole(requestDTO.Email, requestDTO.Role);
            if (!response)
            {
                responseDto.Message = "Failed to assign role";
                responseDto.IsSuccess = false;
                return BadRequest(responseDto);
            }
            responseDto.Message = "Role assigned successfully";
            return Ok(responseDto);
        }

    }
}
