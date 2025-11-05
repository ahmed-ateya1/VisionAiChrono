using MediatR;
using VisionAiChrono.Application.Dtos.VideoDtos;

namespace VisionAiChrono.Application.Slices.Queries.VideoQueries
{
    public record GetVideosByQuery(
        Expression<Func<Video, bool>>? Predicate=null
        , PaginationDto? Pagination = null)
        : IRequest<PaginatedResponse<VideoResponse>>;
    public class GetVideosByQueryHandler (IVideoService videoService) 
        : IRequestHandler<GetVideosByQuery, PaginatedResponse<VideoResponse>>
    {
        public async Task<PaginatedResponse<VideoResponse>> Handle(GetVideosByQuery request, CancellationToken cancellationToken)
        {
            return await videoService.GetVideosBy(request.Predicate,request.Pagination);
        }
    }
}
