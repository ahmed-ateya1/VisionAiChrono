using VisionAiChrono.Application.Dtos.TagDtos;
using VisionAiChrono.Application.Dtos.VideoTagDtos;

namespace VisionAiChrono.Application.ServiceContract
{
    public interface ITagService
    {
        Task<TagResponse> AddTagAsync(TagAddRequest request);
        Task<TagResponse> UpdateTagAsync(TagUpdateRequest request);
        Task<bool> DeleteTagAsync(Guid tagId);
        Task<IEnumerable<TagResponse>> GetAllTagsByAsync(Expression<Func<Tag, bool>>? predicate = null);
        Task<TagResponse?> GetTagByIdAsync(Guid tagId);
        Task<VideoTagResponse> AddTagForVideoAsync(VideoTagAddRequest request);
    }
}
