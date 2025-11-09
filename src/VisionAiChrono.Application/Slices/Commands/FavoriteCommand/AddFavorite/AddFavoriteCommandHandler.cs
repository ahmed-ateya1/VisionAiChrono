using MediatR;
using VisionAiChrono.Application.Dtos.FavoriteDtos;

namespace VisionAiChrono.Application.Slices.Commands.FavoriteCommand
{
    public record AddFavoriteCommand(FavoriteAddRequest Request) : IRequest<FavoriteResponse>;
    
    public class AddFavoriteCommandValidator : AbstractValidator<AddFavoriteCommand>
    {
        public AddFavoriteCommandValidator()
        {
            RuleFor(x => x.Request).NotNull().WithMessage("FavoriteAddRequest cannot be null.");
            RuleFor(x => x.Request.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.Request.PipeleineId).NotEmpty().WithMessage("PipelineId is required.");
        }
    }
    public class AddFavoriteCommandHandler (IFavoriteService favoriteService) 
        : IRequestHandler<AddFavoriteCommand, FavoriteResponse>
    {

        public async Task<FavoriteResponse> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            return await favoriteService.AddFavoriteAsync(request.Request);
        }
    }
}
