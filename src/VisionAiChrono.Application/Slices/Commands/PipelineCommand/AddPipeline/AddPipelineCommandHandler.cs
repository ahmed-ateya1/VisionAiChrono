using MediatR;
using VisionAiChrono.Application.Dtos.PipelineDtos;

namespace VisionAiChrono.Application.Slices.Commands.PipelineCommand
{
    public record AddPipelineCommand(PipelineAddRequest request) : IRequest<PipelineResponse>;

    public class AddPipelineCommandValidator : AbstractValidator<AddPipelineCommand>
    {
        public AddPipelineCommandValidator()
        {
            RuleFor(x => x.request.Title)
                .NotEmpty().WithMessage("Pipeline Title is required.")
                .MaximumLength(100).WithMessage("Pipeline Title must not exceed 100 characters.");
            RuleFor(x => x.request.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.request.ContentJson)
                .MaximumLength(500).WithMessage("ContentJson must not exceed 1000 characters.");
        }
    }
    public class AddPipelineCommandHandler(IPipelineService pipelineService)
        : IRequestHandler<AddPipelineCommand, PipelineResponse>
    {
        public async Task<PipelineResponse> Handle(AddPipelineCommand request, CancellationToken cancellationToken)
        {
            return await pipelineService.CreatePipelineAsync(request.request);
        }
    }
}
