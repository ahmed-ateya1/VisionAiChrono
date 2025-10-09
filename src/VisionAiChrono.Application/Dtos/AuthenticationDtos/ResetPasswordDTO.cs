namespace VisionAiChrono.Application.Dtos.AuthenticationDtos
{
    public class ResetPasswordDTO
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        public string? Email { get; set; }

    }
}
