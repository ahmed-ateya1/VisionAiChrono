using VisionAiChrono.Domain.Models.Identity;

namespace VisionAiChrono.Domain.RepositoryContract
{
    public interface IUserContext
    {
        Task<ApplicationUser?> GetCurrentUserAsync();
    }
}
