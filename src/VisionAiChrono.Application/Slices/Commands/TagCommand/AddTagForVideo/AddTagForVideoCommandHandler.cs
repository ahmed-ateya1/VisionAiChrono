using MediatR;
using VisionAiChrono.Application.Dtos.VideoTagDtos;

namespace VisionAiChrono.Application.Slices.Commands.TagCommand.AddTagForVideo
{
    public record AddTagForVideoCommand(VideoTagAddRequest Request) : IRequest<VideoTagResponse>;
    public class AddTagForVideoCommandHandler(ITagService tagService)
        : IRequestHandler<AddTagForVideoCommand, VideoTagResponse>
    {
        public async Task<VideoTagResponse> Handle(AddTagForVideoCommand request, CancellationToken cancellationToken)
        {
            var result = await tagService.AddTagForVideoAsync(request.Request);
            return result;
        }
    }
}
