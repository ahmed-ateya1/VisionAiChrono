using MediatR;

namespace VisionAiChrono.Application.Slices.Commands
{
    public record DeleteAiModelCommand(Guid id) : IRequest<bool>;
    internal class DeleteAiModelCommandHandler (IModelService modelService)
        : IRequestHandler<DeleteAiModelCommand, bool>
    {
        public Task<bool> Handle(DeleteAiModelCommand request, CancellationToken cancellationToken)
        {
            return modelService.DeleteModelAsync(request.id);
        }
    }
}
