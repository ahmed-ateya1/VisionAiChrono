namespace VisionAiChrono.Application.Dtos.AuthenticationDtos
{
    public class AddRoleDto
    {
        public Guid UserID { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
