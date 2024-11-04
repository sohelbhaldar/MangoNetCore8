using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;

        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _authService.LoginAsync(loginRequestDTO);
            if (loginResponse != null && loginResponse.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(loginResponse.Response));

                await SignInUser(result);
                _tokenProvider.SetToken(result?.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = loginResponse.Message;
                return View(loginRequestDTO);
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() {Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem() {Text = SD.RoleCustomer, Value = SD.RoleCustomer},
            };
            ViewBag.roleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var registerResponse  = await _authService.RegisterAsync(registrationRequestDTO);
            var assignRole = new ResponseDTO();
            if(registerResponse != null && registerResponse.IsSuccess)
            {
                if(string.IsNullOrEmpty(registrationRequestDTO.RoleName))
                {
                    registrationRequestDTO.RoleName = SD.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAsync(registrationRequestDTO);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successfull";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = registerResponse.Message;
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() {Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem() {Text = SD.RoleCustomer, Value = SD.RoleCustomer},
            };
            ViewBag.roleList = roleList;
            return View(registrationRequestDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index","Home");

        }

        private async Task SignInUser(LoginResponseDTO loginResponseDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.ReadJwtToken(loginResponseDTO.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, token?.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, token?.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, token?.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, token?.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, token?.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var claimPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
        }
    }
}
