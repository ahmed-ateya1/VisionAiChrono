using System.ComponentModel.DataAnnotations;

namespace VisionAiChrono.Application.Dtos.AuthenticationDtos
{
    public class OtpVerificationRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Otp { get; set; }
    }
}
