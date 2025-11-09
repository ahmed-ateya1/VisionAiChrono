using VisionAiChrono.Application.Dtos.FavoriteDtos;

namespace VisionAiChrono.Application.Services
{
    public class FavoriteService(IUnitOfWork unitOfWork , ILogger<FavoriteService> logger)
        : IFavoriteService
    {
        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    logger.LogInformation("Starting transaction {TransactionId}", transaction.TransactionId);
                    await action();
                    await unitOfWork.CommitTransactionAsync();
                    logger.LogInformation("Transaction {TransactionId} committed successfully", transaction.TransactionId);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Transaction {TransactionId} failed. Rolling back...", transaction.TransactionId);
                    await unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task<FavoriteResponse> AddFavoriteAsync(FavoriteAddRequest request)
        {
            if (request == null)
            {
                logger.LogError("FavoriteAddRequest is null");
                throw new ArgumentNullException(nameof(request));
            }
            var favorite = request.Adapt<Favourite>();

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Adding favorite for UserId: {UserId}, PipelineId: {PipelineId}", request.UserId, request.PipeleineId);
                await unitOfWork.Repository<Favourite>().CreateAsync(favorite);
            });

           return favorite.Adapt<FavoriteResponse>();
        }

        public async Task<FavoriteResponse> GetFavoriteByIdAsync(Guid favoriteId)
        {
            var favorite = await unitOfWork.Repository<Favourite>()
                .GetByAsync(fa => fa.Id == favoriteId);

            if(favorite == null)
            {
                logger.LogWarning("Favorite with Id: {FavoriteId} not found", favoriteId);
                throw new KeyNotFoundException($"Favorite with Id {favoriteId} not found.");
            }

            return favorite.Adapt<FavoriteResponse>();

        }

        public async Task<PaginatedResponse<FavoriteResponse>> GetFavoritesByAsync
            (Expression<Func<Favourite, bool>>? filter = null, PaginationDto? pagination = null)
        {
            pagination ??= new PaginationDto();

            var favorites = await unitOfWork.Repository<Favourite>()
                .GetAllAsync(filter,
                sortBy: pagination.SortBy,
                sortDirection: pagination.SortDirection,
                pageSize: pagination.PageSize,
                pageIndex: pagination.PageIndex
                );

            var totalCount = favorites.Count();

            if(favorites == null || !favorites.Any())
            {
                logger.LogInformation("No favorites found for the given criteria.");
                return new PaginatedResponse<FavoriteResponse>
                {
                    Items = Array.Empty<FavoriteResponse>(),
                    TotalCount = 0,
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize
                };
            }
            var favoriteResponses = favorites.Adapt<IEnumerable<FavoriteResponse>>();


            return new PaginatedResponse<FavoriteResponse>
            {
                Items = favoriteResponses,
                TotalCount = totalCount,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize
            };
        }

        public async Task<bool> RemoveFavoriteAsync(Guid favoriteId)
        {
            var favorite = await unitOfWork.Repository<Favourite>()
                .GetByAsync(fa => fa.Id == favoriteId);

            if(favorite == null)
            {
                logger.LogWarning("Favorite with Id: {FavoriteId} not found for deletion", favoriteId);
                return false;
            }
            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Removing favorite with Id: {FavoriteId}", favoriteId);
                await unitOfWork.Repository<Favourite>().DeleteAsync(favorite);
            });
            return true;
        }
    }
}
