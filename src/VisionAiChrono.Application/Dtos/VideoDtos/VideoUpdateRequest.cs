namespace VisionAiChrono.Application.Dtos.VideoDtos
{
    public record VideoUpdateRequest
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
    }
}
