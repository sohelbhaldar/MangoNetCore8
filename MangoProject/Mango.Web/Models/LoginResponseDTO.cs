namespace Mango.Web.Models
{
    public class LoginResponseDTO
    {
        public IdentityUserDTO IdentityUser { get; set; }
        public string Token { get; set; }
    }
}
