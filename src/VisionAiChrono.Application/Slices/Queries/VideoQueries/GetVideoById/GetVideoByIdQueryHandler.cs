using MediatR;
using VisionAiChrono.Application.Dtos.VideoDtos;

namespace VisionAiChrono.Application.Slices.Queries.VideoQueries
{
    public record GetVideoByIdQuery(Guid Id) : IRequest<VideoResponse>;
    internal class GetVideoByIdQueryHandler(IVideoService videoService) 
        : IRequestHandler<GetVideoByIdQuery, VideoResponse>
    {
        public async Task<VideoResponse> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
        {
            return await videoService.GetVideoByIdAsync(request.Id);
        }
    }
}
