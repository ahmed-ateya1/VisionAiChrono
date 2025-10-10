using MediatR;
using VisionAiChrono.Application.Dtos.PipelineDtos;

namespace VisionAiChrono.Application.Slices.Queries.PipelineQueries
{
    public record GetPipelinesQuery(PaginationDto? Pagination = null) : IRequest<PaginatedResponse<PipelineResponse>>;
    public class GetPipelinesQueryHandler(IPipelineService pipelineService)
        : IRequestHandler<GetPipelinesQuery, PaginatedResponse<PipelineResponse>>
    {
        public async Task<PaginatedResponse<PipelineResponse>> Handle(GetPipelinesQuery request, CancellationToken cancellationToken)
        {
            return await pipelineService.GetPipelinesByAsync(pagination: request.Pagination);
        }
    }
}
