namespace VisionAiChrono.Infrastructure.Data.Configuration
{
    public class PipelineResultConfiguration : IEntityTypeConfiguration<PipelineResult>
    {
        public void Configure(EntityTypeBuilder<PipelineResult> builder)
        {
            builder.HasKey(pr => pr.Id);

            builder.Property(pr => pr.ResultJson)
                .IsRequired()
                .HasColumnType("text");

            builder.HasOne(pr => pr.PipelineRun)
                .WithMany(run => run.PipelineResults)
                .HasForeignKey(pr => pr.PipelineRunId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pr => pr.Video)
                .WithMany(v => v.PipelineResults)
                .HasForeignKey(pr => pr.VideoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pr => pr.AiModel)
                .WithMany(am => am.PipelineResults)
                .HasForeignKey(pr => pr.AiModelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}