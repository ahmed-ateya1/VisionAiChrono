using MediatR;
using VisionAiChrono.Application.Dtos.AiModelDtos;

namespace VisionAiChrono.Application.Slices.Commands
{
    public record UpdateAiModelCommand(ModelUpdateRequest ModelUpdate) : IRequest<ModelResponse>;

    public class UpdateAiModelValidator : AbstractValidator<UpdateAiModelCommand>
    {
        public UpdateAiModelValidator()
        {
            RuleFor(ai => ai.ModelUpdate)
               .NotNull().WithMessage("Model data is required.");

            RuleFor(ai => ai.ModelUpdate.Name)
                .NotEmpty().WithMessage("AI Model Name is required.")
                .MaximumLength(100).WithMessage("AI Model Name cannot exceed 100 characters.");

            RuleFor(ai => ai.ModelUpdate.Endpoint)
                .NotEmpty().WithMessage("Endpoint URL is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Endpoint must be a valid URL.");

            RuleFor(ai => ai.ModelUpdate.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(ai => ai.ModelUpdate.Version)
                .NotEmpty().WithMessage("Version is required.")
                .MaximumLength(20).WithMessage("Version cannot exceed 20 characters.");

            RuleFor(ai => ai.ModelUpdate.Id)
                .NotNull().WithMessage("id is required.");
        }
    }
    public class UpdateAiModelCommandHandler (IModelService modelService): 
        IRequestHandler<UpdateAiModelCommand, ModelResponse>
    {
        public Task<ModelResponse> Handle(UpdateAiModelCommand request, CancellationToken cancellationToken)
        {
            return modelService.UpdateModelAysnc(request.ModelUpdate);
        }
    }
}
