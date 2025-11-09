namespace VisionAiChrono.Application.Dtos.VideoTagDtos
{
    public record VideoTagAddRequest(Guid VideoId , Guid TagId);
    public record VideoTagResponse(Guid VideoId , Guid TagId);
}
