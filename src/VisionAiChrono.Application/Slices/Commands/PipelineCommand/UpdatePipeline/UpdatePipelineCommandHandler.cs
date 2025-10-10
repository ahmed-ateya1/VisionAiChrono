using MediatR;
using VisionAiChrono.Application.Dtos.PipelineDtos;

namespace VisionAiChrono.Application.Slices.Commands.PipelineCommand
{
    public record UpdatePipelineCommand(PipelineUpdateRequest request) : IRequest<PipelineResponse>;
    
    public class UpdatePipelineCommandValidator : AbstractValidator<UpdatePipelineCommand>
    {
        public UpdatePipelineCommandValidator()
        {
            RuleFor(x => x.request.Id)
                .NotEmpty().WithMessage("Pipeline Id is required.");
            RuleFor(x => x.request.Title)
                .NotEmpty().WithMessage("Pipeline Title is required.")
                .MaximumLength(100).WithMessage("Pipeline Title must not exceed 100 characters.");
            RuleFor(x => x.request.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
            RuleFor(x => x.request.ContentJson)
                .MaximumLength(500).WithMessage("ContentJson must not exceed 1000 characters.");
        }
    }
    public class UpdatePipelineCommandHandler(IPipelineService pipelineService)
        : IRequestHandler<UpdatePipelineCommand, PipelineResponse>
    {
        public async Task<PipelineResponse> Handle(UpdatePipelineCommand request, CancellationToken cancellationToken)
        {
            return await pipelineService.UpdatePipelineAsync(request.request);
        }
    }
}
