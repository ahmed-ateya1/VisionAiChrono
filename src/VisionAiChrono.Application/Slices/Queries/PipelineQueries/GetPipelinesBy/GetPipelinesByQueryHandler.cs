using MediatR;
using VisionAiChrono.Application.Dtos.PipelineDtos;

namespace VisionAiChrono.Application.Slices.Queries.PipelineQueries
{
    public record GetPipelinesByQuery(
        Expression<Func<Pipeline,bool>>? Expression = null,
        PaginationDto? Pagination = null
        ) : IRequest<PaginatedResponse<PipelineResponse>>;
    public class GetPipelinesByQueryHandler(IPipelineService pipelineService)
        : IRequestHandler<GetPipelinesByQuery, PaginatedResponse<PipelineResponse>>
    {
        public async Task<PaginatedResponse<PipelineResponse>> Handle(GetPipelinesByQuery request, CancellationToken cancellationToken)
        {
            return await pipelineService.GetPipelinesByAsync(request.Expression,request.Pagination);
        }
    }
}
