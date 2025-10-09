using VisionAiChrono.Domain.Models.Identity;

namespace VisionAiChrono.Infrastructure.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.SubscriptionType)
                .IsRequired()
                .HasConversion<string>() 
                .HasMaxLength(50);

            builder.Property(u => u.StorageLimitInGB)
                .HasPrecision(18, 4);

            builder.Property(u => u.UsedStorageInGB)
                .HasPrecision(18, 4);

            builder.HasMany(u => u.Videos)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Pipelines)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.AiModels)
                .WithOne(am => am.User)
                .HasForeignKey(am => am.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Favourites)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.PipelineRuns)
                .WithOne(pr => pr.User)
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
