namespace VisionAiChrono.Application.Dtos.FavoriteDtos
{
    public record FavoriteAddRequest(
        Guid UserId,
        Guid PipeleineId
        );
}
