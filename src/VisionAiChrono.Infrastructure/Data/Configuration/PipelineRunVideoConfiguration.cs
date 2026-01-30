
namespace VisionAiChrono.Infrastructure.Data.Configuration
{
    public class PipelineRunVideoConfiguration : IEntityTypeConfiguration<PipelineRunVideo>
    {
        public void Configure(EntityTypeBuilder<PipelineRunVideo> builder)
        {
            builder.HasKey(prv => prv.Id);

            builder.HasOne(prv => prv.PipelineRun)
                   .WithMany(pr => pr.PipelineRunVideos)
                   .HasForeignKey(prv => prv.PipelineRunId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(prv => prv.Video)
                     .WithMany(v => v.PipelineRunVideos)
                     .HasForeignKey(prv => prv.VideoId)
                     .OnDelete(DeleteBehavior.NoAction);

            builder.Property(prv => prv.Id)
                     .IsRequired();

        }
    }
}
