using VisionAiChrono.Application.Dtos.TagDtos;
using VisionAiChrono.Application.Dtos.VideoTagDtos;

namespace VisionAiChrono.Application.Services
{
    public class TagService(IUnitOfWork unitOfWork 
        ,IUserContext userContext
        , ILogger<TagService> logger)
        : ITagService
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
        public async Task<TagResponse> AddTagAsync(TagAddRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request), "TagAddRequest cannot be null");
            }

            var user = await userContext.GetCurrentUserAsync();

            if(user == null)
            {
                logger.LogError("Current user not found in context");
                throw new InvalidOperationException("Current user not found in context");
            }

            var tag = request.Adapt<Tag>();
            tag.UserId = user.Id;

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Tag>().CreateAsync(tag);
            });

            return tag.Adapt<TagResponse>();
        }

        public async Task<bool> DeleteTagAsync(Guid tagId)
        {
            var tag = await unitOfWork.Repository<Tag>()
                .GetByAsync(x => x.Id == tagId);

            if(tag == null)
            {
                logger.LogWarning("Tag with ID {TagId} not found for deletion", tagId);
                return false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Deleting tag with ID {TagId}", tagId);
                await unitOfWork.Repository<Tag>().DeleteAsync(tag);
            });

            return true;
        }

        public async Task<IEnumerable<TagResponse>> GetAllTagsByAsync(Expression<Func<Tag, bool>>? predicate = null)
        {
            var tags = await unitOfWork.Repository<Tag>()
                .GetAllAsync(predicate);

            return tags.Adapt<IEnumerable<TagResponse>>();
        }

        public async Task<TagResponse?> GetTagByIdAsync(Guid tagId)
        {
            var tag = unitOfWork.Repository<Tag>()
                .GetByAsync(x => x.Id == tagId);

            logger.LogInformation("Fetching tag with ID {TagId}", tagId);

            if (tag == null)
            {
                logger.LogError("Tag with ID {TagId} not found", tagId);
                return null;
            }
            return tag.Adapt<TagResponse>();
        }

        public async Task<TagResponse> UpdateTagAsync(TagUpdateRequest request)
        {
            if(request == null)
            {
                logger.LogError("TagUpdateRequest cannot be null");
                throw new ArgumentNullException(nameof(request), "TagUpdateRequest cannot be null");
            }
            var tag = await unitOfWork.Repository<Tag>()
                        .GetByAsync(x => x.Id == request.Id); 

            if(tag == null)
            {
                logger.LogError("Tag with ID {TagId} not found for update", request.Id);
                throw new KeyNotFoundException($"Tag with ID {request.Id} not found");
            }

            request.Adapt(tag);

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Updating tag with ID {TagId}", request.Id);
                await unitOfWork.Repository<Tag>().UpdateAsync(tag);
            });

            return tag.Adapt<TagResponse>();
        }

        public async Task<VideoTagResponse> AddTagForVideoAsync(VideoTagAddRequest request)
        {
            var videoTag = request.Adapt<VideoTag>();

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Adding tag {TagId} to video {VideoId}", request.TagId, request.VideoId);
                await unitOfWork.Repository<VideoTag>().CreateAsync(videoTag);
            });

            return videoTag.Adapt<VideoTagResponse>();
        }
    }
}
