using VisionAiChrono.Application.Dtos.FavoriteDtos;

namespace VisionAiChrono.Application.ServiceContract
{
    public interface IFavoriteService
    {
        Task<FavoriteResponse> AddFavoriteAsync(FavoriteAddRequest request);
        Task<bool> RemoveFavoriteAsync(Guid favoriteId);
        Task<FavoriteResponse> GetFavoriteByIdAsync(Guid favoriteId);
        Task<PaginatedResponse<FavoriteResponse>> GetFavoritesByAsync(Expression<Func<Favourite, bool>>? filter = null, PaginationDto? pagination = null);
    }
}
