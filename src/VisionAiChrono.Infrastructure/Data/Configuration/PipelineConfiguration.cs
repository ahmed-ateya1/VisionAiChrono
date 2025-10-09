namespace VisionAiChrono.Infrastructure.Data.Configuration
{
    public class PipelineConfiguration : IEntityTypeConfiguration<Pipeline>
    {
        public void Configure(EntityTypeBuilder<Pipeline> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.ContentJson)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(p => p.IsPublic)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(u => u.Pipelines)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.BasePipeline)
                .WithMany(p => p.DerivedPipelines)
                .HasForeignKey(p => p.BasePipelineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.DerivedPipelines)
                .WithOne(p => p.BasePipeline)
                .HasForeignKey(p => p.BasePipelineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Favourites)
                .WithOne(f => f.Pipeline)
                .HasForeignKey(f => f.PipelineId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.PipelineModels)
                .WithOne(pm => pm.Pipeline)
                .HasForeignKey(pm => pm.PipelineId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.PipelineRuns)
                .WithOne(pr => pr.Pipeline)
                .HasForeignKey(pr => pr.PipelineId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}