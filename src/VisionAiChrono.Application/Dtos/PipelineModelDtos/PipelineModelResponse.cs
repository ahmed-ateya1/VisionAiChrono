namespace VisionAiChrono.Application.Dtos.PipelineModelDtos
{
    public record PipelineModelResponse(
        Guid Id,
        Guid PipelineId,
        Guid AiModelId,
        int StepOrder,
        DateTime CreatedAt
    );
}
