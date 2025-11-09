using MediatR;
using VisionAiChrono.Application.Dtos.FavoriteDtos;

namespace VisionAiChrono.Application.Slices.Queries.FavoriteQueries
{
    public record GetFavoritesByQuery
        (Expression<Func<Favourite,bool>>? filter = null, PaginationDto? pagination = null) : IRequest<PaginatedResponse<FavoriteResponse>>;
    internal class GetFavoritesByQueryHandler (IFavoriteService favoriteService)
        : IRequestHandler<GetFavoritesByQuery, PaginatedResponse<FavoriteResponse>>
    {
        public Task<PaginatedResponse<FavoriteResponse>> Handle(GetFavoritesByQuery request, CancellationToken cancellationToken)
        {
           return favoriteService.GetFavoritesByAsync(request.filter, request.pagination);
        }
    }
}
