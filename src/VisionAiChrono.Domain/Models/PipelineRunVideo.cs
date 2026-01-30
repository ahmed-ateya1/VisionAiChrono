namespace VisionAiChrono.Domain.Models
{
    public class PipelineRunVideo
    {
        public Guid Id { get; set; }
        public Guid PipelineRunId { get; set; }
        public Guid VideoId { get; set; }
        public virtual PipelineRun PipelineRun { get; set; } = null!;
        public virtual Video Video { get; set; } = null!;

    }
}
