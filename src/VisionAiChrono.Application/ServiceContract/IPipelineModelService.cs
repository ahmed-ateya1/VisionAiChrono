using VisionAiChrono.Application.Dtos.PipelineModelDtos;

namespace VisionAiChrono.Application.ServiceContract
{
    public interface IPipelineModelService
    {
        Task<PipelineModelResponse> CreatePipelineModelAsync(PipelineModelAddRequest request);
        Task<bool> DeletePipelineModelAsync(Guid id);

        Task<PipelineModelResponse?> GetByIdAsync(Guid id);
        Task<List<PipelineModelResponse>> GetPipelineModelsByPipelineIdAsync(Guid pipelineId);

        Task<bool> UpdateModelAsync(Guid pipelineModelId, Guid newModelId);
        Task<bool> UpdateStepOrderAsync(Guid pipelineModelId, int newStepOrder);

        Task<bool> ReorderPipelineStepsAsync(Guid pipelineId, List<StepOrderUpdateRequest> steps);

    }
}
