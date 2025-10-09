namespace VisionAiChrono.Infrastructure.Data.Configuration
{
    public class PipelineRunConfiguration : IEntityTypeConfiguration<PipelineRun>
    {
        public void Configure(EntityTypeBuilder<PipelineRun> builder)
        {
            builder.HasKey(pr => pr.Id);

            builder.Property(pr => pr.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(pr => pr.StartAt)
                .IsRequired();

            builder.Property(pr => pr.EndAt)
                .IsRequired(false);

            builder.HasOne(pr => pr.Pipeline)
                .WithMany(p => p.PipelineRuns)
                .HasForeignKey(pr => pr.PipelineId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pr => pr.User)
                .WithMany(u => u.PipelineRuns)
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(pr => pr.PipelineResults)
                .WithOne(result => result.PipelineRun)
                .HasForeignKey(result => result.PipelineRunId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}