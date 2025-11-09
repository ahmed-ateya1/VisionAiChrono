using MediatR;

namespace VisionAiChrono.Application.Slices.Commands.TagCommand
{
    public record DeleteTagCommand(Guid TagId) : IRequest<bool>;
    public class DeleteTagCommandHandler(ITagService tagService) : IRequestHandler<DeleteTagCommand, bool>
    {
        public async Task<bool> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            return await tagService.DeleteTagAsync(request.TagId);
        }
    }
}
