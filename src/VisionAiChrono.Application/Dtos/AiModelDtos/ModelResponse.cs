namespace VisionAiChrono.Application.Dtos.AiModelDtos
{
    public record ModelResponse(Guid Id, string Name, string Description, string Version, bool IsActive, string Endpoint, DateTime CreatedAt);
}
