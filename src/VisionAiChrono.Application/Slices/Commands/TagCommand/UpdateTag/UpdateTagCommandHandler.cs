using MediatR;
using VisionAiChrono.Application.Dtos.TagDtos;

namespace VisionAiChrono.Application.Slices.Commands.TagCommand
{
    public record UpdateTagCommand(TagUpdateRequest request) : IRequest<TagResponse>;

    public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator()
        {
            RuleFor(x => x.request).NotNull().WithMessage("TagUpdateRequest cannot be null");
            RuleFor(x => x.request.Id).NotEmpty().WithMessage("Tag ID cannot be empty");
            RuleFor(x => x.request.Name)
                .NotEmpty().WithMessage("Tag name cannot be empty")
                .MaximumLength(100).WithMessage("Tag name cannot exceed 100 characters");
        }
    }
    public class UpdateTagCommandHandler(ITagService tagService)
        : IRequestHandler<UpdateTagCommand, TagResponse>
    {
        public Task<TagResponse> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            return tagService.UpdateTagAsync(request.request);
        }
    }
}
