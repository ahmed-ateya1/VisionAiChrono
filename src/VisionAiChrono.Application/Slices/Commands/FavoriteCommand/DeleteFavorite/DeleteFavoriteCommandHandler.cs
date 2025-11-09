using MediatR;

namespace VisionAiChrono.Application.Slices.Commands.FavoriteCommand
{
    public record DeleteFavoriteCommand(Guid FavoriteId) : IRequest<bool>;
    public class DeleteFavoriteCommandHandler(IFavoriteService favoriteService) : IRequestHandler<DeleteFavoriteCommand, bool>
    {
        public async Task<bool> Handle(DeleteFavoriteCommand request, CancellationToken cancellationToken)
        {
            return await favoriteService.RemoveFavoriteAsync(request.FavoriteId);
        }
    }
}
