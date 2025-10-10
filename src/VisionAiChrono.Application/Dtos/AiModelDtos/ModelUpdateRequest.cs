namespace VisionAiChrono.Application.Dtos.AiModelDtos
{
    public record ModelUpdateRequest(Guid Id, string Name, string Description, string Version, bool IsActive, string Endpoint);
}
