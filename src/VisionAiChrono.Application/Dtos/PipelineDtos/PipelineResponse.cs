namespace VisionAiChrono.Application.Dtos.PipelineDtos
{
    public record PipelineResponse(
        Guid Id,
        string Title,
        string Description,
        string ContentJson,
        bool IsPublic,
        Guid UserId,
        string UserName,
        bool IsFavourite,
        bool IsCloned,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
