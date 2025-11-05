using MediatR;
using VisionAiChrono.Application.Dtos.VideoDtos;

namespace VisionAiChrono.Application.Slices.Commands.VideoCommand
{
    public record AddVideoCommand(VideoAddRequest Request) : IRequest<VideoResponse>;

    public class AddVideoValidator : AbstractValidator<AddVideoCommand>
    {
        public AddVideoValidator()
        {
            RuleFor(x => x.Request).NotNull().WithMessage("Video add request cannot be null.");
            RuleFor(x => x.Request.Title)
                .NotEmpty().WithMessage("Video title is required.")
                .MaximumLength(200).WithMessage("Video title cannot exceed 200 characters.");
            RuleFor(x => x.Request.Video)
                .NotNull().WithMessage("Video file is required.")
                .Must(file => file.Length > 0).WithMessage("Video file cannot be empty.");
        }
    }
    public class AddVideoCommandHandler(IVideoService videoService) 
        : IRequestHandler<AddVideoCommand, VideoResponse>
    {
        public async Task<VideoResponse> Handle(AddVideoCommand request, CancellationToken cancellationToken)
        {
            return await videoService.AddVideoAsync(request.Request);
        }
    }
}
