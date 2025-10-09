namespace VisionAiChrono.Application.Dtos.AuthenticationDtos
{
    public class JwtDTO
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInDays { get; set; }
    }
}
