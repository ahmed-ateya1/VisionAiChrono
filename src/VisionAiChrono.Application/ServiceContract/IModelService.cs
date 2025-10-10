using System.Linq.Expressions;
using VisionAiChrono.Application.Dtos;
using VisionAiChrono.Application.Dtos.AiModelDtos;
using VisionAiChrono.Domain.Models;

namespace VisionAiChrono.Application.ServiceContract
{
    public interface IModelService
    {
        Task<ModelResponse> AddModelAsync(ModelAddRequest request);
        Task<ModelResponse> UpdateModelAysnc(ModelUpdateRequest request);
        Task<bool> DeleteModelAsync(Guid modelId);
        Task<ModelResponse> GetModelByAsync(Expression<Func<AiModel, bool>> predicate);
        Task<PaginatedResponse<ModelResponse>> GetModelsAsync(PaginationDto? pagination = null,Expression<Func<AiModel, bool>>? predicate = null);
    }
}
