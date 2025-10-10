using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using VisionAiChrono.Domain.Models.Identity;
using VisionAiChrono.Domain.RepositoryContract;

namespace VisionAiChrono.Infrastructure.Repositories
{
    public class UserContext(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserContext> logger
        )
        : IUserContext
    {
        public async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            var email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                logger.LogWarning("No user is authenticated.");
                return null;
            }

            var user = await unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Email == email);

            return user;
        }
    }
}
