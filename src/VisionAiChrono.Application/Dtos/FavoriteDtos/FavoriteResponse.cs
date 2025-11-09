namespace VisionAiChrono.Application.Dtos.FavoriteDtos
{
    public record FavoriteResponse(
        Guid Id,
        Guid UserId,
        Guid PipeleineId
        );
}
