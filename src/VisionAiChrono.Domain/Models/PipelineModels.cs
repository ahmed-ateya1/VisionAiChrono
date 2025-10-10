namespace VisionAiChrono.Domain.Models
{
    public class PipelineModels
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PipelineId { get; set; }
        public virtual Pipeline Pipeline { get; set; }
        public Guid AiModelId { get; set; }
        public virtual AiModel AiModel { get; set; }
        public int StepOrder { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
