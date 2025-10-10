namespace VisionAiChrono.Domain.Models
{
    public class PipelineResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PipelineRunId { get; set; }
        public virtual PipelineRun PipelineRun { get; set; }
        public Guid VideoId { get; set; }
        public virtual Video Video { get; set; }
        public Guid AiModelId { get; set; }
        public virtual AiModel AiModel { get; set; }
        public string ResultJson { get; set; }
    }
}
