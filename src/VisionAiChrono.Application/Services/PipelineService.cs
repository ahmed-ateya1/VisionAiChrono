using VisionAiChrono.Application.Dtos.PipelineDtos;
using VisionAiChrono.Domain.Models.Identity;

namespace VisionAiChrono.Application.Services
{
    public class PipelineService(
        IUnitOfWork unitOfWork,
        ILogger<PipelineService> logger,
        IUserContext userContext
    ) : IPipelineService
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

        private async Task<IEnumerable<PipelineResponse>> PrepareResponseAsync(IEnumerable<Pipeline> pipelines)
        {
            var userIds = pipelines.Select(p => p.UserId).Distinct().ToList();

            var users = await unitOfWork.Repository<ApplicationUser>()
                .GetAllAsync(u => userIds.Contains(u.Id));

            var userDict = users.ToDictionary(u => u.Id, u => u.UserName ?? string.Empty);

            return pipelines.Select(p => p.Adapt<PipelineResponse>() with
            {
                UserName = userDict.TryGetValue(p.UserId, out var userName) ? userName : string.Empty,
                IsCloned = p.BasePipelineId != null,
                IsFavourite = p.Favourites?.Any() ?? false
            });
        }

        public async Task<PipelineResponse> CreatePipelineAsync(PipelineAddRequest request)
        {
            if (request == null)
            {
                logger.LogError("CreatePipelineAsync called with null request");
                throw new ArgumentNullException(nameof(request));
            }

            var user = await userContext.GetCurrentUserAsync()
                ?? throw new UnauthorizedAccessException("User must be authenticated to add a pipeline");

            var pipeline = request.Adapt<Pipeline>();
            pipeline.UserId = user.Id;
            pipeline.User = user;
            pipeline.CreatedAt = DateTime.UtcNow;

            if (request.BasePipelineId != null)
            {
                var basePipeline = await unitOfWork.Repository<Pipeline>()
                    .GetByAsync(x => x.Id == request.BasePipelineId)
                    ?? throw new PipelineNotFoundException("Base pipeline not found");

                pipeline.BasePipelineId = basePipeline.Id;
            }

            logger.LogInformation("Creating new Pipeline: {PipelineTitle}", pipeline.Title);

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Pipeline>().CreateAsync(pipeline);
            });

            logger.LogInformation("Pipeline {PipelineTitle} added successfully with ID {PipelineId}",
                pipeline.Title, pipeline.Id);

            return pipeline.Adapt<PipelineResponse>() with
            {
                UserName = user.UserName ?? string.Empty
            };
        }

        public async Task<bool> DeletePipelineAsync(Guid id)
        {
            var pipeline = await unitOfWork.Repository<Pipeline>()
                .GetByAsync(x => x.Id == id, includeProperties: "BasePipeline,Favourites,DerivedPipelines");

            if (pipeline == null)
            {
                logger.LogWarning("Pipeline with ID {PipelineId} not found for deletion", id);
                return false;
            }

            if (pipeline.DerivedPipelines?.Any() == true)
                throw new InvalidOperationException("Cannot delete a pipeline that has derived pipelines");

            if (pipeline.Favourites?.Any() == true)
                throw new InvalidOperationException("Cannot delete a pipeline that is marked as favourite");

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Pipeline>().DeleteAsync(pipeline);
            });

            logger.LogInformation("Pipeline {PipelineId} deleted successfully", id);
            return true;
        }

        public async Task<PipelineResponse> GetPipelineByIdAsync(Guid id)
        {
            var pipeline = await unitOfWork.Repository<Pipeline>()
                .GetByAsync(x => x.Id == id, includeProperties: "User,BasePipeline,Favourites,DerivedPipelines")
                ?? throw new PipelineNotFoundException("Pipeline not found");

            var user = await unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Id == pipeline.UserId)
                ?? throw new Exception("User not found");

            return pipeline.Adapt<PipelineResponse>() with
            {
                UserName = user.UserName ?? string.Empty,
                IsCloned = pipeline.BasePipelineId != null,
                IsFavourite = pipeline.Favourites?.Any() ?? false
            };
        }

        public async Task<PaginatedResponse<PipelineResponse>> GetPipelinesByAsync(
            Expression<Func<Pipeline, bool>>? expression = null, 
            PaginationDto? pagination = null)
        {
            pagination ??= new PaginationDto();

            var pipelines = await unitOfWork.Repository<Pipeline>()
                .GetAllAsync(expression,
                    includeProperties: "BasePipeline,Favourites,DerivedPipelines",
                    pageIndex: pagination.PageIndex,
                    pageSize: pagination.PageSize,
                    sortBy: pagination.SortBy,
                    sortDirection: pagination.SortDirection);

            if (pipelines == null || !pipelines.Any())
            {
                logger.LogWarning("No pipelines found with given criteria");
                return new PaginatedResponse<PipelineResponse>
                {
                    TotalCount = 0,
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize,
                    Items = Enumerable.Empty<PipelineResponse>()
                };
            }

            var totalCount = await unitOfWork.Repository<Pipeline>().CountAsync(expression);
            var response = await PrepareResponseAsync(pipelines);

            return new PaginatedResponse<PipelineResponse>
            {
                Items = response,
                TotalCount = totalCount,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize
            };
        }

        public async Task<PipelineResponse> UpdatePipelineAsync(PipelineUpdateRequest request)
        {
            if (request == null)
            {
                logger.LogError("UpdatePipelineAsync called with null request");
                throw new ArgumentNullException(nameof(request));
            }

            var existingPipeline = await unitOfWork.Repository<Pipeline>()
                .GetByAsync(x => x.Id == request.Id)
                ?? throw new PipelineNotFoundException($"Pipeline with ID {request.Id} not found");

            var updatedPipeline = request.Adapt(existingPipeline);

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Updating pipeline {PipelineId} - {PipelineTitle}",
                    updatedPipeline.Id, updatedPipeline.Title);
                updatedPipeline.UpdatedAt = DateTime.UtcNow;
                await unitOfWork.Repository<Pipeline>().UpdateAsync(updatedPipeline);
            });

            var user = await unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Id == updatedPipeline.UserId);

            return updatedPipeline.Adapt<PipelineResponse>() with
            {
                UserName = user?.UserName ?? string.Empty,
                IsCloned = updatedPipeline.BasePipelineId != null,
                IsFavourite = updatedPipeline.Favourites?.Any() ?? false
            };
        }
    }
}
