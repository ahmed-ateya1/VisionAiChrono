namespace VisionAiChrono.Application.Dtos.VideoDtos
{
    public record VideoResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Url { get; init; }
        public long SizeInBytes { get; init; }
        public TimeSpan Duration { get; init; }
        public DateTime UploadedAt { get; init; }

    }
}
