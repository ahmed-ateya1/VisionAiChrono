namespace VisionAiChrono.Application.Dtos.AiModelDtos
{
    public record ModelAddRequest(string Name, string Description, string Version, bool IsActive, string Endpoint);
}
