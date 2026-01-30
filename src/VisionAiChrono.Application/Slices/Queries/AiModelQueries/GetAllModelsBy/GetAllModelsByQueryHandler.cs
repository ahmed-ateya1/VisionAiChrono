using MediatR;
using VisionAiChrono.Application.Dtos.AiModelDtos;

namespace VisionAiChrono.Application.Slices.Queries
{ 
    public record GetAllModelsByQuery(
        PaginationDto? PaginationDto 
        , Expression<Func<AiModel,bool>> Expression
        ) : IRequest<PaginatedResponse<ModelResponse>>;
    public class GetAllModelsByQueryHandler(IModelService modelService)
        : IRequestHandler<GetAllModelsByQuery, PaginatedResponse<ModelResponse>>
    {
        public async Task<PaginatedResponse<ModelResponse>> Handle(GetAllModelsByQuery request, CancellationToken cancellationToken)
        {
            return await modelService.GetModelsAsync(request.PaginationDto,request.Expression);
        }
    }
}
