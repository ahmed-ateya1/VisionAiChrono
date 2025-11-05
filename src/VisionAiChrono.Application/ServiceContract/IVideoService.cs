using VisionAiChrono.Application.Dtos.VideoDtos;

namespace VisionAiChrono.Application.ServiceContract
{
    public interface IVideoService
    {
        Task<VideoResponse> AddVideoAsync(VideoAddRequest request);
        Task<VideoResponse> GetVideoByIdAsync(Guid videoId);
        Task<PaginatedResponse<VideoResponse>> GetVideosBy(Expression<Func<Video, bool>>? predicate = null, PaginationDto? paginationDto = null);
        Task<bool> DeleteVideoAsync(Guid videoId);
    }
}
