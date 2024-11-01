using Mango.Service.AuthAPI.Models.DTO;
using Mango.Service.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDTO _responseDTO;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDTO = new();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            var errorMessage = await _authService.Register(registrationRequestDTO);
            if (!string.IsNullOrEmpty(errorMessage))
            { 
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = errorMessage;
                return BadRequest(_responseDTO);
            }
            return Ok(_responseDTO);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _authService.Login(loginRequestDTO);
            if (loginResponse?.IdentityUser == null)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "UserName or Password is incorrect";
                return BadRequest(_responseDTO);
            }
            else
            {
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Login Successfull";
                _responseDTO.Response = loginResponse;
            }
            return Ok(_responseDTO);
        }

        [HttpPost("assignrole")]
        public async Task<IActionResult> AssignRole(RoleRequestDTO roleRequestDTO)
        {
            var loginResponse = await _authService.AssignRole(roleRequestDTO.Email, roleRequestDTO.RoleName.ToUpper());
            if (!loginResponse)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Error encountered";
                return BadRequest(_responseDTO);
            }
            else
            {
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Role Assigned Successfull";
                _responseDTO.Response = loginResponse;
            }
            return Ok(_responseDTO);
        }
    }
}
