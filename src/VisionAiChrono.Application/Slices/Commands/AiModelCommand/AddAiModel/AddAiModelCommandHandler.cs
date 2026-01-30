using MediatR;
using VisionAiChrono.Application.Dtos.AiModelDtos;

namespace VisionAiChrono.Application.Slices.Commands
{
    public record AddAiModelCommand(ModelAddRequest ModelAdd) : IRequest<ModelResponse>;

    public class AddAiModelValidator : AbstractValidator<AddAiModelCommand>
    {
        public AddAiModelValidator()
        {
            RuleFor(ai => ai.ModelAdd)
                .NotNull().WithMessage("Model data is required.");

            RuleFor(ai => ai.ModelAdd.Name)
                .NotEmpty().WithMessage("AI Model Name is required.")
                .MaximumLength(100).WithMessage("AI Model Name cannot exceed 100 characters.");

            RuleFor(ai => ai.ModelAdd.Endpoint)
                .NotEmpty().WithMessage("Endpoint URL is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Endpoint must be a valid URL.");

            RuleFor(ai => ai.ModelAdd.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(ai => ai.ModelAdd.Version)
                .NotEmpty().WithMessage("Version is required.")
                .MaximumLength(20).WithMessage("Version cannot exceed 20 characters.");

        }
    }
    public class AddAiModelCommandHandler 
        (IModelService modelService)
        : IRequestHandler<AddAiModelCommand, ModelResponse>
    {
        public Task<ModelResponse> Handle(AddAiModelCommand request, CancellationToken cancellationToken)
        {
            return modelService.AddModelAsync(request.ModelAdd);
        }
    }
}
