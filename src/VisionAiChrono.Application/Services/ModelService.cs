using VisionAiChrono.Application.Dtos.AiModelDtos;

namespace VisionAiChrono.Application.Services
{
    public class ModelService(
        IUnitOfWork unitOfWork,
        ILogger<ModelService> logger,
        IUserContext userContext
        )
        : IModelService
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
        public async Task<ModelResponse> AddModelAsync(ModelAddRequest request)
        {
            if (request == null)
            {
                logger.LogError("AddModelAsync called with null request");
                throw new ArgumentNullException(nameof(request));
            }
            var user = await userContext.GetCurrentUserAsync();
            if (user == null)
            {
                logger.LogError("No authenticated user found in context");
                throw new UnauthorizedAccessException("User must be authenticated to add a model");
            }
            var model = request.Adapt<AiModel>();
            model.UserId = user.Id;

            logger.LogInformation("Adding new model: {ModelName}", model.Name);

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Creating model in database: {ModelName}", model.Name);
                await unitOfWork.Repository<AiModel>().CreateAsync(model);
            });

            logger.LogInformation("Model {ModelName} added successfully with ID {ModelId}", model.Name, model.Id);
            return model.Adapt<ModelResponse>();
        }

        public async Task<bool> DeleteModelAsync(Guid modelId)
        {
            var model = await unitOfWork.Repository<AiModel>().GetByAsync(x=>x.Id == modelId);

            if(model == null)
            {
                logger.LogWarning("Model with ID {ModelId} not found for deletion", modelId);
                return false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Deleting model with ID {ModelId}", modelId);
                await unitOfWork.Repository<AiModel>().DeleteAsync(model);
            });

            return true;
        }

        public async Task<ModelResponse> GetModelByAsync(Expression<Func<AiModel, bool>> predicate)
        {
            var model = await unitOfWork.Repository<AiModel>()
                .GetByAsync(predicate);

            if(model == null)
            {
                logger.LogWarning("Model not found with given predicate");
                throw new ModelNotFoundException("Model not found");
            }

            return model.Adapt<ModelResponse>();
        }

        public async Task<PaginatedResponse<ModelResponse>> 
            GetModelsAsync(PaginationDto? pagination = null, Expression<Func<AiModel, bool>>? predicate = null)
        {
            pagination ??= new PaginationDto();

            var query = await unitOfWork.Repository<AiModel>()
                .GetAllAsync(predicate ,
                pageIndex: pagination.PageIndex, 
                pageSize:pagination.PageSize,
                sortBy: pagination.SortBy ,
                sortDirection: pagination.SortDirection);


            if(query == null || !query.Any())
            {
                logger.LogWarning("No models found with given criteria");
                return new PaginatedResponse<ModelResponse>
                {
                   
                    TotalCount = 0,
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize,
                    Items = Enumerable.Empty<ModelResponse>()
                };
            }
            var totalCount = await unitOfWork.Repository<AiModel>()
                .CountAsync(predicate);


            var modelResponses = query.Adapt<IEnumerable<ModelResponse>>();


            return new PaginatedResponse<ModelResponse>
            {
                Items = modelResponses,
                TotalCount = totalCount,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize
            };
        }

        public async Task<ModelResponse> UpdateModelAysnc(ModelUpdateRequest request)
        {
            if (request == null)
            {
                logger.LogError("UpdateModelAysnc called with null request");
                throw new ArgumentNullException(nameof(request));
            }

            var aiModel = await unitOfWork.Repository<AiModel>()
                .GetByAsync(x => x.Id == request.Id);

            if (aiModel == null)
            {
                logger.LogWarning("Model with ID {ModelId} not found for update", request.Id);
                throw new ModelNotFoundException($"Model with ID {request.Id} not found");
            }

            request.Adapt(aiModel);

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Updating model with ID {ModelId}", aiModel.Id);
                await unitOfWork.Repository<AiModel>().UpdateAsync(aiModel);
            });

            return aiModel.Adapt<ModelResponse>();
        }
    }
}
