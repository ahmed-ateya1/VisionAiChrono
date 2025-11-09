using MediatR;
using VisionAiChrono.Application.Dtos.TagDtos;

namespace VisionAiChrono.Application.Slices.Queries.TagQueries
{
    public record GetTagsByQuery(Expression<Func<Tag, bool>> Predicate) : IRequest<IEnumerable<TagResponse>>;
    public class GetTagsByQueryHandler(ITagService tagService)
        : IRequestHandler<GetTagsByQuery, IEnumerable<TagResponse>>
    {
        public async Task<IEnumerable<TagResponse>> Handle(GetTagsByQuery request, CancellationToken cancellationToken)
        {
            return await tagService.GetAllTagsByAsync(request.Predicate);
        }
    }
}
