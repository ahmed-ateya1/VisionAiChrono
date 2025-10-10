namespace VisionAiChrono.Application.Dtos.PipelineDtos
{
    public record PipelineUpdateRequest(
        Guid Id,
        string Title,
        string Description,
        string ContentJson,
        bool IsPublic,
        Guid? BasePipelineId
    );
}
