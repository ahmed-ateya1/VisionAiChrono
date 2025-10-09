using System.ComponentModel.DataAnnotations;

namespace VisionAiChrono.Application.Dtos.AuthenticationDtos
{
    public class LoginDTO
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
