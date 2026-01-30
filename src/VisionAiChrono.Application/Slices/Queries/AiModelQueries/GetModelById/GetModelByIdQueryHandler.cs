using MediatR;
using VisionAiChrono.Application.Dtos.AiModelDtos;

namespace VisionAiChrono.Application.Slices.Queries
{ 
    public record GetModelByIdQuery(Guid id) : IRequest<ModelResponse>;
    internal class GetModelByIdQueryHandler(IModelService modelService)
        : IRequestHandler<GetModelByIdQuery, ModelResponse>
    {
        public async Task<ModelResponse> Handle(GetModelByIdQuery request, CancellationToken cancellationToken)
        {
            return await modelService.GetModelByAsync(x=>x.Id == request.id);
        }
    }
}
