using MediatR;
using VisionAiChrono.Application.Dtos.TagDtos;

namespace VisionAiChrono.Application.Slices.Queries.TagQueries
{
    public record GetTagByIdQuery(Guid TagId) : IRequest<TagResponse>;
    public class GetTagByIdQueryHandler(ITagService tagService)
        : IRequestHandler<GetTagByIdQuery, TagResponse>
    {
        public async Task<TagResponse> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            return await tagService.GetTagByIdAsync(request.TagId);
        }
    }
}
