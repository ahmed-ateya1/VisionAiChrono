namespace VisionAiChrono.Application.Dtos.AuthenticationDtos
{
    public class UserDto
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string SubscriptionType { get; set; }
        public decimal StorageLimitInGB { get; set; }
        public decimal UsedStorageInGB { get; set; }
        public List<string> Roles { get; set; }
    }
}
