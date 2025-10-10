using MediatR;
using VisionAiChrono.Application.Dtos.PipelineDtos;

namespace VisionAiChrono.Application.Slices.Queries.PipelineQueries
{
    public record GetPipelineByIdQuery(Guid Id) : IRequest<PipelineResponse>;
    internal class GetPipelineByIdQueryHandler(IPipelineService pipelineService)
        : IRequestHandler<GetPipelineByIdQuery, PipelineResponse>
    {
        public async Task<PipelineResponse> Handle(GetPipelineByIdQuery request, CancellationToken cancellationToken)
        {

            return await pipelineService.GetPipelineByIdAsync(request.Id);
        }
    }
}
