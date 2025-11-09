using VisionAiChrono.Application.Dtos.VideoDtos;
using Xabe.FFmpeg;

namespace VisionAiChrono.Application.Services
{
    public class VideoService(IUnitOfWork unitOfWork,
        IUserContext userContext,
        ILogger<VideoService> logger,
        IFileServices fileServices) 
        : IVideoService
    {

        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
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

    public async Task<VideoResponse> AddVideoAsync(VideoAddRequest request)
    {
        if (request == null)
        {
            logger.LogError("VideoAddRequest is null.");
            throw new ArgumentNullException(nameof(request));
        }

        var user = await userContext.GetCurrentUserAsync()
            ?? throw new UnauthorizedAccessException("User must be authenticated to add a video");


        var video = request.Adapt<Video>();
        video.Url = await fileServices.CreateFile(request.Video);
        video.SizeInBytes = request.Video.Length;
        video.UploadedAt = DateTime.UtcNow;
        video.UserId = user.Id;


        video.Duration = TimeSpan.MinValue;

        await ExecuteWithTransactionAsync(async () =>
        {
            await unitOfWork.Repository<Video>().CreateAsync(video);
        });

        return video.Adapt<VideoResponse>();
    }


    public async Task<bool> DeleteVideoAsync(Guid videoId)
        {
            var video = await unitOfWork.Repository<Video>().GetByAsync(x => x.Id == videoId);

            if(video == null)
            {
                logger.LogWarning("Video with ID {VideoId} not found for deletion.", videoId);
                return false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Deleting video with ID {VideoId}", videoId);
                await unitOfWork.Repository<Video>().DeleteAsync(video);
                await fileServices.DeleteFile(video.Url);
            });
            return true;
        }

        public async Task<VideoResponse> GetVideoByIdAsync(Guid videoId)
        {
            var video = await unitOfWork.Repository<Video>().GetByAsync(x => x.Id == videoId);

            if(video == null)
            {
                logger.LogWarning("Video with ID {VideoId} not found.", videoId);
                throw new KeyNotFoundException($"Video with ID {videoId} not found.");
            }
            return video.Adapt<VideoResponse>();
        }

        public async Task<PaginatedResponse<VideoResponse>> GetVideosBy(Expression<Func<Video, bool>>? predicate = null, PaginationDto? paginationDto = null)
        {
            paginationDto ??= new PaginationDto();
            var videos = await unitOfWork.Repository<Video>().GetAllAsync(
                predicate,
                sortBy: paginationDto.SortBy,
                sortDirection: paginationDto.SortDirection,
                pageIndex: paginationDto.PageIndex, 
                pageSize: paginationDto.PageSize
               );

            if (videos == null || !videos.Any())
                {
                logger.LogInformation("No videos found matching the given criteria.");
                return new PaginatedResponse<VideoResponse>
                {
                    Items = Array.Empty<VideoResponse>(),
                    TotalCount = 0,
                    PageIndex = paginationDto.PageIndex,
                    PageSize = paginationDto.PageSize
                };
            }
            var totalCount = await unitOfWork.Repository<Video>().CountAsync(predicate);

           var videoResponses =  videos.Adapt<IEnumerable<VideoResponse>>();


            return new PaginatedResponse<VideoResponse>
            {
                Items = videoResponses,
                TotalCount = totalCount,
                PageIndex = paginationDto.PageIndex,
                PageSize = paginationDto.PageSize
            };

        }
    }
}
