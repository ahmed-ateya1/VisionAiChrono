using MediatR;
using VisionAiChrono.Application.Dtos.AiModelDtos;

namespace VisionAiChrono.Application.Slices.Queries
{
    public record GetAllModelsQuery(PaginationDto? PaginationDto): IRequest<PaginatedResponse<ModelResponse>>;
    public class GetAllModelsQueryHandler(IModelService modelService) 
        : IRequestHandler<GetAllModelsQuery, PaginatedResponse<ModelResponse>>
    {
        public async Task<PaginatedResponse<ModelResponse>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
        {
            return await modelService.GetModelsAsync(request.PaginationDto);
        }
    }
}
