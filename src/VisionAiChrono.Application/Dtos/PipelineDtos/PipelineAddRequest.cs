namespace VisionAiChrono.Application.Dtos.PipelineDtos
{
    public record PipelineAddRequest(
        string Title,
        string Description,
        string ContentJson,
        bool IsPublic,
        Guid? BasePipelineId
    );
}
