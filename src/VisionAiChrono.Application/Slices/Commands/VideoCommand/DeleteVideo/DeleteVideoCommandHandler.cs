using MediatR;

namespace VisionAiChrono.Application.Slices.Commands.VideoCommand
{
    public record DeleteVideoCommand(Guid VideoId) : IRequest<bool>;
    public class DeleteVideoCommandHandler(IVideoService videoService) 
        : IRequestHandler<DeleteVideoCommand, bool>
    {
        public async Task<bool> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
        {
            return await videoService.DeleteVideoAsync(request.VideoId);
        }

    }
}
