using VisionAiChrono.Domain.Models.Identity;

namespace VisionAiChrono.Domain.Models
{
    public class AiModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } 
        public string Description { get; set; }
        public string Version { get; set; }
        public bool IsActive { get; set; } 
        public string Endpoint { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<PipelineModels> PipelineModels { get; set; } = [];
        public virtual ICollection<PipelineResult> PipelineResults { get; set; } = [];

    }
}
