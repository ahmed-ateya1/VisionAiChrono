namespace VisionAiChrono.Application.Dtos.PipelineModelDtos
{
    public record StepOrderUpdateRequest(
        Guid PipelineModelId,
        int NewStepOrder
    );
}
