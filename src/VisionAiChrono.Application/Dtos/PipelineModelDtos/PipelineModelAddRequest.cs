namespace VisionAiChrono.Application.Dtos.PipelineModelDtos
{
    public record PipelineModelAddRequest(
        Guid PipelineId,
        Guid AiModelId,
        int StepOrder
    );
}
