using MediatR;
using VisionAiChrono.Application.Dtos.TagDtos;

namespace VisionAiChrono.Application.Slices.Commands.TagCommand
{
    public record CreateTagCommand(TagAddRequest request) : IRequest<TagResponse>;

    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.request).NotNull().WithMessage("TagAddRequest cannot be null");
            RuleFor(x => x.request.Name)
                .NotEmpty().WithMessage("Tag name cannot be empty")
                .MaximumLength(100).WithMessage("Tag name cannot exceed 100 characters");
        }
    }
    public class CreateTagCommandHandler(ITagService tagService)
        : IRequestHandler<CreateTagCommand, TagResponse>
    {
        public async Task<TagResponse> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            return await tagService.AddTagAsync(request.request);
        }
    }
}
