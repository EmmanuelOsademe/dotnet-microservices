using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Services.IServices;
using EMStores.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EMStores.Web.Controllers
{
    public class AuthController(IAuthService authService, ITokenProvider tokenProvider) : Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new ();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO requestDTO)
        {
            var res = await _authService.LoginAsync(requestDTO);
            
            if (res.IsSuccess)
            {
                LoginResponseDTO? loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(res.Result));

                if (loginResponse != null)
                {
                    await SignInUser(loginResponse);
                    _tokenProvider.SetToken(loginResponse.Token);
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
				TempData["error"] = res.Message ?? "Error registering user";
				ModelState.AddModelError("CustomError", res?.Message??string.Empty);
            }
            

            return View(requestDTO);
        }


        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new (){Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin},
                new (){Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var result = await _authService.RegisterAsync(registrationRequestDTO);
            ResponseDto? assignRole;
            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDTO.Role))
                {
                    registrationRequestDTO.Role = StaticDetails.RoleCustomer;
                }

                assignRole = await _authService.AssignRoleAsync(registrationRequestDTO);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
				TempData["error"] = result.Message??"Error registering user";
				ModelState.AddModelError("CustomError", result?.Message ?? string.Empty);
            }
            var roleList = new List<SelectListItem>()
            {
                new (){Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin},
                new (){Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer},
            };
            ViewBag.RoleList = roleList;

            return View(registrationRequestDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDTO loginDto)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(loginDto.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));


            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
