using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
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
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() {Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem() {Text = SD.RoleCustomer, Value = SD.RoleCustomer},
            };
            ViewBag.roleList = roleList;
            return View(registrationRequestDTO);
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
