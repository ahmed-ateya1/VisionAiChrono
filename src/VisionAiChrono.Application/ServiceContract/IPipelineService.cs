using VisionAiChrono.Application.Dtos.PipelineDtos;

namespace VisionAiChrono.Application.ServiceContract
{
    public interface IPipelineService
    {
        Task<PaginatedResponse<PipelineResponse>> GetPipelinesByAsync(Expression<Func<Pipeline, bool>>? expression = null, PaginationDto? pagination = null);
        Task<PipelineResponse> GetPipelineByIdAsync(Guid id);
        Task<PipelineResponse> CreatePipelineAsync(PipelineAddRequest request);
        Task<PipelineResponse> UpdatePipelineAsync(PipelineUpdateRequest request);
        Task<bool> DeletePipelineAsync(Guid id);
    }
}
