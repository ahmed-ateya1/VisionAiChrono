using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using VisionAiChrono.Domain.Models.Identity;
using VisionAiChrono.Infrastructure.Data.Configuration;

namespace VisionAiChrono.Infrastructure.Data
{
    public class ApplicationDbContext 
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {

        public DbSet<AiModel> AiModels { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Pipeline> Pipelines { get; set; }
        public DbSet<PipelineModels> PipelineModels { get; set; }
        public DbSet<PipelineResult> PipelineResults { get; set; }
        public DbSet<PipelineRun> PipelineRuns { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoTag> VideoTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AiModelConfiguration).Assembly);
        }
    }
}
