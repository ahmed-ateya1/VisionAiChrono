using Microsoft.AspNetCore.Http;

namespace VisionAiChrono.Application.Dtos.VideoDtos
{
    public record VideoAddRequest
    {
        public string Title { get; init; }
        public IFormFile Video { get; init; }
    }
}
