using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using VisionAiChrono.Application.ServiceContract;
using VisionAiChrono.Application.Services;

namespace VisionAiChrono.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add application services here, e.g., email sender, authentication services, etc.
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAuthenticationServices,AuthenticationServices>();

            return services;
        }
    }
}
