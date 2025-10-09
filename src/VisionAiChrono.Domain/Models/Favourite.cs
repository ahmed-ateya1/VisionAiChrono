using VisionAiChrono.Domain.Models.Identity;

namespace VisionAiChrono.Domain.Models
{
    public class Favourite
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Guid PipelineId { get; set; }
        public virtual Pipeline Pipeline { get; set; }
    }
}
