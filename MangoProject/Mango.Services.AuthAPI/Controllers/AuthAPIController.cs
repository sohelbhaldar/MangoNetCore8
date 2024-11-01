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
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
    }
}
