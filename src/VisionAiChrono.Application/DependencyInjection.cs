using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using VisionAiChrono.Application.Services;
using VisionAiChrono.Application.Slices.Commands;

namespace VisionAiChrono.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add application services here, e.g., email sender, authentication services, etc.
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAuthenticationServices,AuthenticationServices>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IPipelineService, PipelineService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IFileServices,FileService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddValidatorsFromAssemblyContaining<AddAiModelValidator>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<AddAiModelCommand>();
            });

            return services;
        }
    }
}
