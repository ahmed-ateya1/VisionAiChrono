using Microsoft.AspNetCore.Identity;
using VisionAiChrono.Domain.Enums;

namespace VisionAiChrono.Domain.Models.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Free;
        public decimal StorageLimitInGB { get; set; }
        public decimal UsedStorageInGB { get; set; }
        public string? OTPCode { get; set; }
        public DateTime? OTPExpiration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<AiModel> AiModels { get; set; } = [];
        public virtual ICollection<Video> Videos { get; set; } = [];
        public virtual ICollection<Pipeline> Pipelines { get; set; } = [];
        public virtual ICollection<Favourite> Favourites { get; set; } = [];
        public virtual ICollection<PipelineRun> PipelineRuns { get; set; } = [];
    }
}
