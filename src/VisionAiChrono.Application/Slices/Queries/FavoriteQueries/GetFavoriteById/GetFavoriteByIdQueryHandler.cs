using MediatR;
using VisionAiChrono.Application.Dtos.FavoriteDtos;

namespace VisionAiChrono.Application.Slices.Queries.FavoriteQueries
{
    public record GetFavoriteByIdQuery(Guid FavoriteId) : IRequest<FavoriteResponse>;
    public class GetFavoriteByIdQueryHandler (IFavoriteService favoriteService) 
        : IRequestHandler<GetFavoriteByIdQuery, FavoriteResponse>
    {
        public async Task<FavoriteResponse> Handle(GetFavoriteByIdQuery request, CancellationToken cancellationToken)
        {
           return await favoriteService.GetFavoriteByIdAsync(request.FavoriteId);
        }
    }

}
