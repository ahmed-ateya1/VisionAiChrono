using MediatR;
using VisionAiChrono.Application.Dtos.PipelineModelDtos;

namespace VisionAiChrono.Application.Services
{
    public class PipelineModelService(
        IUnitOfWork unitOfWork,
        ILogger<PipelineModelService> logger)
        : IPipelineModelService
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



        private async Task NormalizeStepOrderAsync(Guid pipelineId)
        {
            var steps = await unitOfWork.Repository<PipelineModels>()
                .GetAllAsync(x => x.PipelineId == pipelineId);

            var ordered = steps
                .OrderBy(x => x.StepOrder)
                .ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                ordered[i].StepOrder = i + 1;
                await unitOfWork.Repository<PipelineModels>().UpdateAsync(ordered[i]);
            }
        }



        public async Task<PipelineModelResponse> CreatePipelineModelAsync(PipelineModelAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var pipeline = await unitOfWork.Repository<Pipeline>()
                .GetByAsync(x => x.Id == request.PipelineId);

            if (pipeline == null)
                throw new PipelineNotFoundException($"Pipeline with ID {request.PipelineId} does not exist.");

            var aiModel = await unitOfWork.Repository<AiModel>()
                .GetByAsync(x => x.Id == request.AiModelId);

            if (aiModel == null)
                throw new AiModelNotFoundException($"AI Model with ID {request.AiModelId} does not exist.");

            var pipelineModels = await unitOfWork.Repository<PipelineModels>()
                        .GetAllAsync(x => x.PipelineId == request.PipelineId);

            var maxStepOrder = pipelineModels.Any()
                ? pipelineModels.Max(x => x.StepOrder)
                : 0;


            var pipelineModel = request.Adapt<PipelineModels>();
            pipelineModel.StepOrder = maxStepOrder + 1;

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<PipelineModels>().CreateAsync(pipelineModel);
            });

            return pipelineModel.Adapt<PipelineModelResponse>();
        }



        public async Task<bool> DeletePipelineModelAsync(Guid id)
        {
            var pipelineModel = await unitOfWork.Repository<PipelineModels>()
                .GetByAsync(x => x.Id == id);

            if (pipelineModel == null)
                return false;

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<PipelineModels>().DeleteAsync(pipelineModel);
                await NormalizeStepOrderAsync(pipelineModel.PipelineId);
            });

            return true;
        }



        public async Task<PipelineModelResponse?> GetByIdAsync(Guid id)
        {
            var pipelineModel = await unitOfWork.Repository<PipelineModels>()
                .GetByAsync(x => x.Id == id);

            return pipelineModel?.Adapt<PipelineModelResponse>();
        }

        public async Task<List<PipelineModelResponse>> GetPipelineModelsByPipelineIdAsync(Guid pipelineId)
        {
            var models = await unitOfWork.Repository<PipelineModels>()
                .GetAllAsync(x => x.PipelineId == pipelineId);

            return models
                .OrderBy(x => x.StepOrder)
                .Adapt<List<PipelineModelResponse>>();
        }



        public async Task<bool> UpdateModelAsync(Guid pipelineModelId, Guid newModelId)
        {
            var pipelineModel = await unitOfWork.Repository<PipelineModels>()
                .GetByAsync(x => x.Id == pipelineModelId);

            if (pipelineModel == null)
                return false;

            var aiModel = await unitOfWork.Repository<AiModel>()
                .GetByAsync(x => x.Id == newModelId);

            if (aiModel == null)
                return false;

            pipelineModel.AiModelId = newModelId;

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<PipelineModels>().UpdateAsync(pipelineModel);
            });

            return true;
        }



        public async Task<bool> UpdateStepOrderAsync(Guid pipelineModelId, int newStepOrder)
        {
            var target = await unitOfWork.Repository<PipelineModels>()
                .GetByAsync(x => x.Id == pipelineModelId);

            if (target == null)
                return false;

            var steps = await unitOfWork.Repository<PipelineModels>()
                .GetAllAsync(x => x.PipelineId == target.PipelineId);

            if (newStepOrder < 1 || newStepOrder > steps.Count())
                throw new InvalidOperationException("Invalid StepOrder value.");

            await ExecuteWithTransactionAsync(async () =>
            {
                var ordered = steps
                    .OrderBy(x => x.StepOrder)
                    .ToList();

                ordered.Remove(target);
                ordered.Insert(newStepOrder - 1, target);

                for (int i = 0; i < ordered.Count; i++)
                {
                    ordered[i].StepOrder = i + 1;
                    await unitOfWork.Repository<PipelineModels>().UpdateAsync(ordered[i]);
                }
            });

            return true;
        }



        public async Task<bool> ReorderPipelineStepsAsync(
            Guid pipelineId,
            List<StepOrderUpdateRequest> steps)
        {
            var pipelineModels = await unitOfWork.Repository<PipelineModels>()
                .GetAllAsync(x => x.PipelineId == pipelineId);

            if (!pipelineModels.Any())
                return false;

            // Validation
            if (steps.Select(x => x.NewStepOrder).Distinct().Count() != steps.Count)
                throw new InvalidOperationException("Duplicate StepOrder detected.");

            var expected = Enumerable.Range(1, steps.Count).ToList();
            var actual = steps.Select(x => x.NewStepOrder).OrderBy(x => x).ToList();

            if (!expected.SequenceEqual(actual))
                throw new InvalidOperationException("StepOrder must be continuous from 1 to N.");

            var ids = pipelineModels.Select(x => x.Id).ToHashSet();
            if (!steps.All(x => ids.Contains(x.PipelineModelId)))
                throw new InvalidOperationException("Invalid PipelineModelId detected.");

            await ExecuteWithTransactionAsync(async () =>
            {
                foreach (var step in steps)
                {
                    var model = pipelineModels.First(x => x.Id == step.PipelineModelId);
                    model.StepOrder = step.NewStepOrder;
                    await unitOfWork.Repository<PipelineModels>().UpdateAsync(model);
                }
            });

            return true;
        }

    }
}
