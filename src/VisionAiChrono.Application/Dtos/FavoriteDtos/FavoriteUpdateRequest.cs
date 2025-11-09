namespace VisionAiChrono.Application.Dtos.FavoriteDtos
{
    public record FavoriteUpdateRequest(
        Guid Id,
        Guid UserId,
        Guid PipeleineId
        );
}
