using MediatR;

namespace VisionAiChrono.Application.Slices.Commands.PipelineCommand
{
    public record DeletePipelineCommand(Guid PipelineId) : IRequest<bool>;
    public class DeletePipelineCommandHandler(IPipelineService pipelineService)
        : IRequestHandler<DeletePipelineCommand, bool>
    {
        public async Task<bool> Handle(DeletePipelineCommand request, CancellationToken cancellationToken)
        {
            return await pipelineService.DeletePipelineAsync(request.PipelineId);
        }
    }
}
