using VisionAiChrono.Domain.Enums;
using VisionAiChrono.Domain.Models.Identity;

namespace VisionAiChrono.Domain.Models
{
    public class PipelineRun
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PipelineId { get; set; }
        public virtual Pipeline Pipeline { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public PipelineStatus Status { get; set; } = PipelineStatus.Pending;
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public virtual ICollection<PipelineResult> PipelineResults { get; set; } = [];
    }
}
